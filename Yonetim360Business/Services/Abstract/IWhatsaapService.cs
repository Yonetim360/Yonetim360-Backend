using Yonetim360Business.DTO.WhatsAppDtos;

namespace Yonetim360Business.Services.Abstract
{
    public interface IWhatsAppService
    {
        Task<WhatsAppSendResultDto> SendAsync(WhatsAppOutboundMessageDto message, CancellationToken cancellationToken = default);
    }
}
