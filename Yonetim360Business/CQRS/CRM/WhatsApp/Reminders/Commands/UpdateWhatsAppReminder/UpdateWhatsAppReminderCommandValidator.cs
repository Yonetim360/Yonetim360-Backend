using FluentValidation;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.UpdateWhatsAppReminder
{
    public class UpdateWhatsAppReminderCommandValidator : AbstractValidator<UpdateWhatsAppReminderCommand>
    {
        public UpdateWhatsAppReminderCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.UpdatedBy).NotEmpty();
            RuleFor(x => x.RecipientName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.RecipientPhoneNumber).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(4000).When(x => !x.TemplateId.HasValue);
            RuleFor(x => x.ScheduledAt).NotEmpty();
        }
    }
}
