using Microsoft.Extensions.Configuration;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class WhatsAppMessageDispatcher : IWhatsAppMessageDispatcher
    {
        private readonly IWhatsAppService _whatsAppService;
        private readonly IConfiguration _configuration;

        public WhatsAppMessageDispatcher(IWhatsAppService whatsAppService, IConfiguration configuration)
        {
            _whatsAppService = whatsAppService;
            _configuration = configuration;
        }

        public async Task DispatchAsync(WhatsAppMessage message, WhatsAppSettings? settings, CancellationToken cancellationToken = default)
        {
            var result = await _whatsAppService.SendAsync(new WhatsAppOutboundMessageDto
            {
                ToPhoneNumber = message.RecipientPhoneNumber,
                FromPhoneNumber = settings?.FromPhoneNumber,
                MessagingServiceSid = settings?.MessagingServiceSid,
                Body = string.IsNullOrWhiteSpace(message.ProviderContentSid) ? message.Body : null,
                ContentSid = message.ProviderContentSid,
                ContentVariablesJson = message.ContentVariablesJson,
                StatusCallbackUrl = BuildStatusCallbackUrl()
            }, cancellationToken);

            ApplyResult(message, result);
        }

        private string? BuildStatusCallbackUrl()
        {
            var explicitUrl = _configuration["Twilio:StatusCallbackUrl"];
            if (!string.IsNullOrWhiteSpace(explicitUrl))
                return explicitUrl;

            var baseUrl = _configuration["Twilio:StatusCallbackBaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                return null;

            return $"{baseUrl.TrimEnd('/')}/api/WhatsApp/webhooks/twilio/status";
        }

        private static void ApplyResult(WhatsAppMessage message, WhatsAppSendResultDto result)
        {
            message.ProviderMessageSid = result.ProviderMessageSid;
            message.ProviderErrorCode = result.ErrorCode;
            message.ProviderErrorMessage = result.ErrorMessage;
            message.Status = result.Status;

            var now = DateTime.UtcNow;
            if (result.Status == WhatsAppMessageStatus.Failed)
            {
                message.FailedAt = now;
                return;
            }

            message.SentAt = now;
            message.FailedAt = null;
        }
    }
}
