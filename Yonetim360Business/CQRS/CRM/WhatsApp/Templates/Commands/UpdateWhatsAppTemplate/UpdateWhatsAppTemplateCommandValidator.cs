using FluentValidation;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.UpdateWhatsAppTemplate
{
    public class UpdateWhatsAppTemplateCommandValidator : AbstractValidator<UpdateWhatsAppTemplateCommand>
    {
        public UpdateWhatsAppTemplateCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.UpdatedBy).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Category).MaximumLength(100);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.ProviderContentSid).MaximumLength(128);
        }
    }
}
