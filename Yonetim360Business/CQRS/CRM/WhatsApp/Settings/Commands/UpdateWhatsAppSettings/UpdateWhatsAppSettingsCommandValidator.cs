using FluentValidation;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Settings.Commands.UpdateWhatsAppSettings
{
    public class UpdateWhatsAppSettingsCommandValidator : AbstractValidator<UpdateWhatsAppSettingsCommand>
    {
        public UpdateWhatsAppSettingsCommandValidator()
        {
            RuleFor(x => x.UpdatedBy).NotEmpty();
            RuleFor(x => x.FromPhoneNumber).MaximumLength(32);
            RuleFor(x => x.MessagingServiceSid).MaximumLength(128);
            RuleFor(x => x.MaxRetryAttempts).InclusiveBetween(1, 10);
            RuleFor(x => x.RetryDelayMinutes).InclusiveBetween(1, 120);
        }
    }
}
