using FluentValidation;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.DeleteUploadedDocument
{
    public class DeleteUploadedDocumentCommandValidator : AbstractValidator<DeleteUploadedDocumentCommand>
    {
        public DeleteUploadedDocumentCommandValidator()
        {
            RuleFor(x => x.UploadedDocumentId)
                .NotEmpty()
                .WithMessage("Uploaded document id is required.");
        }
    }
}
