using Microsoft.AspNetCore.Http;
using Yonetim360Business.DTO.DocumentProcessing;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.CreateUploadedDocument
{
    public class CreateUploadedDocumentCommand : ICommand<UploadedDocumentDto>
    {
        public IFormFile File { get; set; } = null!;
        public Guid? OwnerId { get; set; }
        public string? SourceModule { get; set; }
    }
}
