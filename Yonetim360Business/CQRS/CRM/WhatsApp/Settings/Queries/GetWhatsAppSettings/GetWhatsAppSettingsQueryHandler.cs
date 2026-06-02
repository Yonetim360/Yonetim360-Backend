using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Settings.Queries.GetWhatsAppSettings
{
    public class GetWhatsAppSettingsQueryHandler : IQueryHandler<GetWhatsAppSettingsQuery, WhatsAppSettingsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public GetWhatsAppSettingsQueryHandler(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<WhatsAppSettingsDto> Handle(GetWhatsAppSettingsQuery request, CancellationToken cancellationToken)
        {
            var settings = await _context.WhatsAppSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken)
                ?? new WhatsAppSettings
                {
                    Id = Guid.Empty,
                    AutoSendEnabled = true,
                    NotificationsEnabled = true,
                    MaxRetryAttempts = 3,
                    RetryDelayMinutes = 5
                };

            return WhatsAppMapping.ToDto(settings, _configuration);
        }
    }
}
