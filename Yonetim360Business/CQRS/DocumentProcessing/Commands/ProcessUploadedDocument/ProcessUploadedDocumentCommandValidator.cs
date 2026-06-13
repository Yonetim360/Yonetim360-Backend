using FluentValidation;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.ProcessUploadedDocument
{
    public class ProcessUploadedDocumentCommandValidator : AbstractValidator<ProcessUploadedDocumentCommand>
    {
        public ProcessUploadedDocumentCommandValidator()
        {
            RuleFor(x => x.UploadedDocumentId)
                .NotEmpty()
                .WithMessage("Uploaded document id is required.");
        }
    }
}
