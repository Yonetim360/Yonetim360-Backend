using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.DeleteWhatsAppReminder
{
    public class DeleteWhatsAppReminderCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
