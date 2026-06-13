using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.DTO.DocumentProcessing;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.CreateUploadedDocument
{
    public class CreateUploadedDocumentCommandHandler : ICommandHandler<CreateUploadedDocumentCommand, UploadedDocumentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UploadedDocument> _uploadedDocumentRepository;
        private readonly IDocumentStorageService _documentStorageService;
        private readonly ICurrentUserService _currentUserService;

        public CreateUploadedDocumentCommandHandler(
            IUnitOfWork unitOfWork,
            IDocumentStorageService documentStorageService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _uploadedDocumentRepository = unitOfWork.GetRepository<UploadedDocument>();
            _documentStorageService = documentStorageService;
            _currentUserService = currentUserService;
        }

        public async Task<UploadedDocumentDto> Handle(CreateUploadedDocumentCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();
            var userId = _currentUserService.GetUserId();
            var storageResult = await _documentStorageService.SaveUploadedDocumentAsync(tenantId, request.File, cancellationToken);

            var document = new UploadedDocument
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                OriginalFileName = request.File.FileName,
                StoredFileName = storageResult.StoredFileName,
                StoredPath = storageResult.StoredPath,
                MimeType = request.File.ContentType ?? "application/pdf",
                Extension = Path.GetExtension(request.File.FileName),
                FileSize = request.File.Length,
                OwnerId = request.OwnerId,
                SourceModule = request.SourceModule,
                IsProcessed = false
            };

            await _uploadedDocumentRepository.CreateAsync(document);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new UploadedDocumentDto
            {
                Id = document.Id,
                OriginalFileName = document.OriginalFileName,
                StoredFileName = document.StoredFileName,
                StoredPath = document.StoredPath,
                FileSize = document.FileSize,
                MimeType = document.MimeType,
                Extension = document.Extension,
                IsProcessed = document.IsProcessed,
                ProcessStatus = DocumentProcessStatus.Uploaded
            };
        }
    }
}
