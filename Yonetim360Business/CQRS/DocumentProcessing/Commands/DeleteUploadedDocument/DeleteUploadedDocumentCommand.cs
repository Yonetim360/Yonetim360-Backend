using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.DeleteUploadedDocument
{
    public class DeleteUploadedDocumentCommand : ICommand<bool>
    {
        public Guid UploadedDocumentId { get; set; }
    }
}
