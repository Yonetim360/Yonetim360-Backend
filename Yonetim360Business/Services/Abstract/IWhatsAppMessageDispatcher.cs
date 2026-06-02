using Yonetim360.Entity.CRM;

namespace Yonetim360Business.Services.Abstract
{
    public interface IWhatsAppMessageDispatcher
    {
        Task DispatchAsync(WhatsAppMessage message, WhatsAppSettings? settings, CancellationToken cancellationToken = default);
    }
}
