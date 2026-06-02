using FluentValidation;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Commands.SendWhatsAppMessage
{
    public class SendWhatsAppMessageCommandValidator : AbstractValidator<SendWhatsAppMessageCommand>
    {
        public SendWhatsAppMessageCommandValidator()
        {
            RuleFor(x => x.CreatedBy).NotEmpty();
            RuleFor(x => x.RecipientName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.RecipientPhoneNumber).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(4000).When(x => !x.TemplateId.HasValue);
        }
    }
}
