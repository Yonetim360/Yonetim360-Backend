using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.UpdateWhatsAppReminder
{
    public class UpdateWhatsAppReminderCommandHandler : ICommandHandler<UpdateWhatsAppReminderCommand, WhatsAppMessageDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateWhatsAppReminderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WhatsAppMessageDto> Handle(UpdateWhatsAppReminderCommand request, CancellationToken cancellationToken)
        {
            var reminder = await _context.WhatsAppMessages
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.MessageType == WhatsAppMessageType.Reminder, cancellationToken)
                ?? throw new InvalidDataException("WhatsApp reminder not found.");

            if (reminder.Status != WhatsAppMessageStatus.Scheduled)
                throw new InvalidDataException("Only scheduled WhatsApp reminders can be updated.");

            var settings = await _context.WhatsAppSettings.FirstOrDefaultAsync(cancellationToken);
            var template = request.TemplateId.HasValue
                ? await _context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Id == request.TemplateId.Value, cancellationToken)
                : null;
            var providerContentSid = template?.IsApproved == true ? template.ProviderContentSid : null;

            if (string.IsNullOrWhiteSpace(providerContentSid) &&
                settings?.AllowFreeFormScheduledMessages != true)
            {
                throw new InvalidDataException("Scheduled WhatsApp reminders require an approved Twilio ContentSid.");
            }

            reminder.UpdatedBy = request.UpdatedBy;
            reminder.CustomerId = request.CustomerId;
            reminder.TemplateId = template?.Id;
            reminder.RecipientName = request.RecipientName;
            reminder.RecipientPhoneNumber = request.RecipientPhoneNumber;
            reminder.Body = template?.Content ?? request.Body;
            reminder.ContentVariablesJson = request.ContentVariablesJson;
            reminder.ProviderContentSid = providerContentSid;
            reminder.ScheduledAt = request.ScheduledAt;

            await _context.SaveChangesAsync(cancellationToken);
            reminder.Template = template;

            return WhatsAppMapping.ToDto(reminder);
        }
    }
}
