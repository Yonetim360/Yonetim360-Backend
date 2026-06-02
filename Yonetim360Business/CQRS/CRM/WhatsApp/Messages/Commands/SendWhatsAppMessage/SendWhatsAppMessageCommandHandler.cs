using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Commands.SendWhatsAppMessage
{
    public class SendWhatsAppMessageCommandHandler : ICommandHandler<SendWhatsAppMessageCommand, WhatsAppMessageDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IWhatsAppMessageDispatcher _dispatcher;

        public SendWhatsAppMessageCommandHandler(ApplicationDbContext context, IWhatsAppMessageDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task<WhatsAppMessageDto> Handle(SendWhatsAppMessageCommand request, CancellationToken cancellationToken)
        {
            var template = request.TemplateId.HasValue
                ? await _context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Id == request.TemplateId.Value, cancellationToken)
                : null;

            var settings = await _context.WhatsAppSettings.FirstOrDefaultAsync(cancellationToken);
            var body = template?.Content ?? request.Body;
            var providerContentSid = template?.IsApproved == true ? template.ProviderContentSid : null;

            var message = new WhatsAppMessage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
                CustomerId = request.CustomerId,
                TemplateId = template?.Id,
                RecipientName = request.RecipientName,
                RecipientPhoneNumber = request.RecipientPhoneNumber,
                Body = body,
                ContentVariablesJson = request.ContentVariablesJson,
                ProviderContentSid = providerContentSid,
                MessageType = WhatsAppMessageType.Manual,
                Status = WhatsAppMessageStatus.Queued
            };

            if (template != null)
                template.UsageCount++;

            _context.WhatsAppMessages.Add(message);
            await _context.SaveChangesAsync(cancellationToken);

            await _dispatcher.DispatchAsync(message, settings, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return WhatsAppMapping.ToDto(message);
        }
    }
}
