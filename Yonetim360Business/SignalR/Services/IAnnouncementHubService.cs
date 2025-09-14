using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.SignalR.Services
{
    public interface IAnnouncementHubService
    {
        Task NotifyPersonelAssigned(List<Guid> personelIds, object announcementData);
        Task NotifySpecificPersonel(Guid personelId, object notification);
    }
}
