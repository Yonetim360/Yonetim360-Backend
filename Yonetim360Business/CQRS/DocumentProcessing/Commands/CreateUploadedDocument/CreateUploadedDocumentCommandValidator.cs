using FluentValidation;

namespace Yonetim360Business.CQRS.DocumentProcessing.Commands.CreateUploadedDocument
{
    public class CreateUploadedDocumentCommandValidator : AbstractValidator<CreateUploadedDocumentCommand>
    {
        private static readonly string[] AllowedExtensions = [".pdf"];

        public CreateUploadedDocumentCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");

            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .When(x => x.File != null)
                .WithMessage("File cannot be empty.");

            RuleFor(x => x.File.FileName)
                .Must(fileName => AllowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
                .When(x => x.File != null)
                .WithMessage("Only PDF files are supported.");
        }
    }
}
