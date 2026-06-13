using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.DTO.DocumentProcessing;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.ProcessUploadedDocument
{
    public class ProcessUploadedDocumentCommandHandler : ICommandHandler<ProcessUploadedDocumentCommand, ProcessedDocumentResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UploadedDocument> _uploadedDocumentRepository;
        private readonly IRepository<ProcessedDocument> _processedDocumentRepository;
        private readonly IRepository<AnalyzedPage> _analyzedPageRepository;
        private readonly IRepository<ExtractedDocument> _extractedDocumentRepository;
        private readonly IDocumentOcrService _documentOcrService;
        private readonly IDocumentClassificationService _documentClassificationService;
        private readonly IPageGroupingService _pageGroupingService;
        private readonly IPdfSplitService _pdfSplitService;
        private readonly IDocumentStorageService _documentStorageService;
        private readonly ICurrentUserService _currentUserService;

        public ProcessUploadedDocumentCommandHandler(
            IUnitOfWork unitOfWork,
            IDocumentOcrService documentOcrService,
            IDocumentClassificationService documentClassificationService,
            IPageGroupingService pageGroupingService,
            IPdfSplitService pdfSplitService,
            IDocumentStorageService documentStorageService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _uploadedDocumentRepository = unitOfWork.GetRepository<UploadedDocument>();
            _processedDocumentRepository = unitOfWork.GetRepository<ProcessedDocument>();
            _analyzedPageRepository = unitOfWork.GetRepository<AnalyzedPage>();
            _extractedDocumentRepository = unitOfWork.GetRepository<ExtractedDocument>();
            _documentOcrService = documentOcrService;
            _documentClassificationService = documentClassificationService;
            _pageGroupingService = pageGroupingService;
            _pdfSplitService = pdfSplitService;
            _documentStorageService = documentStorageService;
            _currentUserService = currentUserService;
        }

        public async Task<ProcessedDocumentResultDto> Handle(ProcessUploadedDocumentCommand request, CancellationToken cancellationToken)
        {
            var uploadedDocument = await _uploadedDocumentRepository.GetFirstOrDefaultAsync(x => x.Id == request.UploadedDocumentId)
                ?? throw new FileNotFoundException($"Uploaded document with id {request.UploadedDocumentId} could not be found.");

            if (!File.Exists(uploadedDocument.StoredPath))
            {
                throw new FileNotFoundException("Stored PDF file could not be found on disk.");
            }

            var currentUserId = _currentUserService.GetUserId();
            var ocrTexts = await _documentOcrService.ExtractTextPerPageAsync(uploadedDocument.StoredPath);
            var fullText = string.Join(" ", ocrTexts);
            var (detectedDocumentType, confidence) = _documentClassificationService.ClassifyWithConfidence(fullText);

            var processedDocument = new ProcessedDocument
            {
                Id = Guid.NewGuid(),
                TenantId = uploadedDocument.TenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserId,
                UploadedDocumentId = uploadedDocument.Id,
                FilePath = uploadedDocument.StoredPath,
                PageCount = ocrTexts.Count,
                OcrTextSample = ocrTexts.FirstOrDefault(),
                IsProcessed = true,
                Notes = string.Empty,
                DetectedDocumentType = detectedDocumentType,
                ClassificationConfidence = confidence,
                DetectedKeywords = $"General: {detectedDocumentType} ({confidence:P})",
                ProcessStatus = DocumentProcessStatus.Processed
            };

            await _processedDocumentRepository.CreateAsync(processedDocument);

            var analyzedPages = new List<AnalyzedPage>();
            var pageNumber = 1;

            foreach (var text in ocrTexts)
            {
                var (pageType, pageConfidence) = _documentClassificationService.ClassifyWithConfidence(text);
                var analyzedPage = new AnalyzedPage
                {
                    Id = Guid.NewGuid(),
                    TenantId = uploadedDocument.TenantId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUserId,
                    ProcessedDocumentId = processedDocument.Id,
                    PageNumber = pageNumber,
                    DetectedDocumentType = pageType,
                    ClassificationConfidence = pageConfidence,
                    DetectedKeywords = pageType == DocumentFileType.BatchPdf
                        ? $"Page {pageNumber}: Unclassified"
                        : $"Page {pageNumber}: {pageType} ({pageConfidence:P})"
                };

                analyzedPages.Add(analyzedPage);
                await _analyzedPageRepository.CreateAsync(analyzedPage);
                pageNumber++;
            }

            var pageGroups = _pageGroupingService.GroupConsecutivePages(analyzedPages, 0.3);
            for (var index = 0; index < pageGroups.Count; index++)
            {
                foreach (var page in pageGroups[index].Pages)
                {
                    page.DocumentGroupId = index;
                }
            }

            var outputDirectory = _documentStorageService.EnsureExtractedDirectory(uploadedDocument.TenantId, uploadedDocument.Id);
            var extractedPdfs = await _pdfSplitService.SplitPdfByDocumentTypeAsync(uploadedDocument.StoredPath, pageGroups, outputDirectory);

            var extractedDocumentDtos = new List<ExtractedDocumentDto>();
            foreach (var extractedPdf in extractedPdfs)
            {
                var extractedDocument = new ExtractedDocument
                {
                    Id = Guid.NewGuid(),
                    TenantId = uploadedDocument.TenantId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUserId,
                    ProcessedDocumentId = processedDocument.Id,
                    DocumentType = extractedPdf.DocumentType,
                    ExtractedPdfPath = extractedPdf.FilePath,
                    ExtractedFileName = extractedPdf.FileName,
                    StartPageNumber = extractedPdf.StartPage,
                    EndPageNumber = extractedPdf.EndPage,
                    PageCount = extractedPdf.PageCount,
                    AverageConfidence = extractedPdf.AverageConfidence,
                    CombinedKeywords = extractedPdf.CombinedKeywords,
                    IsSuccessfullyExtracted = extractedPdf.IsSuccessful,
                    ExtractionNotes = extractedPdf.ErrorMessage
                };

                await _extractedDocumentRepository.CreateAsync(extractedDocument);

                extractedDocumentDtos.Add(new ExtractedDocumentDto
                {
                    Id = extractedDocument.Id,
                    DocumentType = extractedDocument.DocumentType,
                    ExtractedFileName = extractedDocument.ExtractedFileName,
                    ExtractedPdfPath = extractedDocument.ExtractedPdfPath,
                    StartPageNumber = extractedDocument.StartPageNumber,
                    EndPageNumber = extractedDocument.EndPageNumber,
                    PageCount = extractedDocument.PageCount,
                    AverageConfidence = extractedDocument.AverageConfidence,
                    IsSuccessfullyExtracted = extractedDocument.IsSuccessfullyExtracted,
                    ExtractionNotes = extractedDocument.ExtractionNotes
                });
            }

            uploadedDocument.IsProcessed = true;
            processedDocument.ProcessStatus = DocumentProcessStatus.Processed;

            await _uploadedDocumentRepository.UpdateAsync(uploadedDocument);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new ProcessedDocumentResultDto
            {
                ProcessedDocumentId = processedDocument.Id,
                UploadedDocumentId = uploadedDocument.Id,
                PageCount = processedDocument.PageCount,
                DetectedDocumentType = processedDocument.DetectedDocumentType,
                ClassificationConfidence = processedDocument.ClassificationConfidence,
                ExtractedDocuments = extractedDocumentDtos
            };
        }
    }
}
