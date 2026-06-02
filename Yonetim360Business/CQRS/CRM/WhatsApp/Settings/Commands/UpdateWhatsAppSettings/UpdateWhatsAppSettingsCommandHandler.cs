using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Settings.Commands.UpdateWhatsAppSettings
{
    public class UpdateWhatsAppSettingsCommandHandler : ICommandHandler<UpdateWhatsAppSettingsCommand, WhatsAppSettingsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UpdateWhatsAppSettingsCommandHandler(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<WhatsAppSettingsDto> Handle(UpdateWhatsAppSettingsCommand request, CancellationToken cancellationToken)
        {
            var settings = await _context.WhatsAppSettings.FirstOrDefaultAsync(cancellationToken)
                ?? WhatsAppMapping.CreateDefaultSettings(request.UpdatedBy);

            if (settings.Id == Guid.Empty || _context.Entry(settings).State == EntityState.Detached)
                _context.WhatsAppSettings.Add(settings);

            settings.UpdatedBy = request.UpdatedBy;
            settings.FromPhoneNumber = request.FromPhoneNumber;
            settings.MessagingServiceSid = request.MessagingServiceSid;
            settings.DefaultTemplateId = request.DefaultTemplateId;
            settings.AutoSendEnabled = request.AutoSendEnabled;
            settings.NotificationsEnabled = request.NotificationsEnabled;
            settings.MaxRetryAttempts = request.MaxRetryAttempts;
            settings.RetryDelayMinutes = request.RetryDelayMinutes;
            settings.AllowFreeFormScheduledMessages = request.AllowFreeFormScheduledMessages;

            await _context.SaveChangesAsync(cancellationToken);
            return WhatsAppMapping.ToDto(settings, _configuration);
        }
    }
}
