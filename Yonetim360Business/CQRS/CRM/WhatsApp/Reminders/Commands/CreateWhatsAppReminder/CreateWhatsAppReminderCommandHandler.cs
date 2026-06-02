using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.CreateWhatsAppReminder
{
    public class CreateWhatsAppReminderCommandHandler : ICommandHandler<CreateWhatsAppReminderCommand, WhatsAppMessageDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IWhatsAppMessageDispatcher _dispatcher;

        public CreateWhatsAppReminderCommandHandler(ApplicationDbContext context, IWhatsAppMessageDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task<WhatsAppMessageDto> Handle(CreateWhatsAppReminderCommand request, CancellationToken cancellationToken)
        {
            var settings = await _context.WhatsAppSettings.FirstOrDefaultAsync(cancellationToken);
            var template = request.TemplateId.HasValue
                ? await _context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Id == request.TemplateId.Value, cancellationToken)
                : null;
            var providerContentSid = template?.IsApproved == true ? template.ProviderContentSid : null;

            if (!request.SendNow &&
                string.IsNullOrWhiteSpace(providerContentSid) &&
                settings?.AllowFreeFormScheduledMessages != true)
            {
                throw new InvalidDataException("Scheduled WhatsApp reminders require an approved Twilio ContentSid.");
            }

            var message = new WhatsAppMessage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
                CustomerId = request.CustomerId,
                TemplateId = template?.Id,
                RecipientName = request.RecipientName,
                RecipientPhoneNumber = request.RecipientPhoneNumber,
                Body = template?.Content ?? request.Body,
                ContentVariablesJson = request.ContentVariablesJson,
                ProviderContentSid = providerContentSid,
                MessageType = WhatsAppMessageType.Reminder,
                Status = request.SendNow ? WhatsAppMessageStatus.Queued : WhatsAppMessageStatus.Scheduled,
                ScheduledAt = request.SendNow ? DateTime.UtcNow : request.ScheduledAt
            };

            if (template != null)
                template.UsageCount++;

            _context.WhatsAppMessages.Add(message);
            await _context.SaveChangesAsync(cancellationToken);

            if (request.SendNow)
            {
                await _dispatcher.DispatchAsync(message, settings, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return WhatsAppMapping.ToDto(message);
        }
    }
}
