using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.DeleteUploadedDocument
{
    public class DeleteUploadedDocumentCommandHandler : ICommandHandler<DeleteUploadedDocumentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UploadedDocument> _uploadedDocumentRepository;
        private readonly IRepository<ProcessedDocument> _processedDocumentRepository;
        private readonly IRepository<AnalyzedPage> _analyzedPageRepository;
        private readonly IRepository<ExtractedDocument> _extractedDocumentRepository;

        public DeleteUploadedDocumentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _uploadedDocumentRepository = unitOfWork.GetRepository<UploadedDocument>();
            _processedDocumentRepository = unitOfWork.GetRepository<ProcessedDocument>();
            _analyzedPageRepository = unitOfWork.GetRepository<AnalyzedPage>();
            _extractedDocumentRepository = unitOfWork.GetRepository<ExtractedDocument>();
        }

        public async Task<bool> Handle(DeleteUploadedDocumentCommand request, CancellationToken cancellationToken)
        {
            var uploadedDocument = await _uploadedDocumentRepository.GetFirstOrDefaultAsync(x => x.Id == request.UploadedDocumentId)
                ?? throw new FileNotFoundException($"Uploaded document with id {request.UploadedDocumentId} could not be found.");

            var processedDocuments = (await _processedDocumentRepository.GetAllAsync(x => x.UploadedDocumentId == uploadedDocument.Id, tracked: true, pageSize: 0)).ToList();
            foreach (var processedDocument in processedDocuments)
            {
                var analyzedPages = await _analyzedPageRepository.GetAllAsync(x => x.ProcessedDocumentId == processedDocument.Id, tracked: true, pageSize: 0);
                foreach (var analyzedPage in analyzedPages)
                {
                    await _analyzedPageRepository.DeleteAsync(analyzedPage);
                }

                var extractedDocuments = await _extractedDocumentRepository.GetAllAsync(x => x.ProcessedDocumentId == processedDocument.Id, tracked: true, pageSize: 0);
                foreach (var extractedDocument in extractedDocuments)
                {
                    await _extractedDocumentRepository.DeleteAsync(extractedDocument);
                }

                await _processedDocumentRepository.DeleteAsync(processedDocument);
            }

            await _uploadedDocumentRepository.DeleteAsync(uploadedDocument);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
