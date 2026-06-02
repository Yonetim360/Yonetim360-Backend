using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.DeleteWhatsAppReminder
{
    public class DeleteWhatsAppReminderCommandHandler : ICommandHandler<DeleteWhatsAppReminderCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteWhatsAppReminderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteWhatsAppReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await _context.WhatsAppMessages
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.MessageType == WhatsAppMessageType.Reminder, cancellationToken)
                ?? throw new InvalidDataException("WhatsApp reminder not found.");

            reminder.Status = WhatsAppMessageStatus.Cancelled;
            _context.WhatsAppMessages.Remove(reminder);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
