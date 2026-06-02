using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.UpdateWhatsAppReminder
{
    public class UpdateWhatsAppReminderCommand : ICommand<WhatsAppMessageDto>
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? TemplateId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPhoneNumber { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ContentVariablesJson { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
