using Yonetim360Business.DTO.DocumentProcessing;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.ProcessUploadedDocument
{
    public class ProcessUploadedDocumentCommand : ICommand<ProcessedDocumentResultDto>
    {
        public Guid UploadedDocumentId { get; set; }
    }
}
