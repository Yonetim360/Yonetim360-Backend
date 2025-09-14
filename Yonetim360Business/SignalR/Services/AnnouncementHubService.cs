using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.SignalR.Services
{
    public class AnnouncementHubService : IAnnouncementHubService
    {
        private readonly IHubContext<AnnouncementHub> _hubContext;
        private readonly ILogger<AnnouncementHubService> _logger;
        public AnnouncementHubService(IHubContext<AnnouncementHub> hubContext, ILogger<AnnouncementHubService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyPersonelAssigned(List<Guid> personelIds, object announcementData)
        {
            try
            {
                var notification = new
                {
                    Data = announcementData,
                    Message = "Yeni bir duyurunuz var !",
                    Type = "announcement_assigned",
                    Timestamp = DateTime.UtcNow
                };

                // Her personele ayrı ayrı bildirim gönder
                foreach (var personelId in personelIds)
                {
                    await _hubContext.Clients.Group($"Personel_{personelId}")
                        .SendAsync("NewAnnouncementAssigned", notification);
                }

                _logger.LogInformation($"Assignment notification sent to {personelIds.Count} personnel");

                // Admin'e de başarılı atama bilgisi
                var adminNotification = new
                {
                    AssignedCount = personelIds.Count,
                    PersonelIds = personelIds,
                    AnnouncementData = announcementData,
                    Message = $"{personelIds.Count} personele duyuru atandı",
                    Type = "assignment_completed",
                    Timestamp = DateTime.UtcNow
                };

                await _hubContext.Clients.Group("Administrators")
                    .SendAsync("AssignmentCompleted", adminNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending assignment notifications");
            }
        }

        public async Task NotifySpecificPersonel(Guid personelId, object notification)
        {
            try
            {
                await _hubContext.Clients.Group($"Personel_{personelId}")
                    .SendAsync("PersonalNotification", notification);

                _logger.LogInformation($"Personal notification sent to personel: {personelId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending personal notification to personel: {personelId}");
            }
        }
    }
}
