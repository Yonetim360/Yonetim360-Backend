using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Settings.Commands.UpdateWhatsAppSettings
{
    public class UpdateWhatsAppSettingsCommand : ICommand<WhatsAppSettingsDto>
    {
        public Guid UpdatedBy { get; set; }
        public string? FromPhoneNumber { get; set; }
        public string? MessagingServiceSid { get; set; }
        public Guid? DefaultTemplateId { get; set; }
        public bool AutoSendEnabled { get; set; } = true;
        public bool NotificationsEnabled { get; set; } = true;
        public int MaxRetryAttempts { get; set; } = 3;
        public int RetryDelayMinutes { get; set; } = 5;
        public bool AllowFreeFormScheduledMessages { get; set; }
    }
}
