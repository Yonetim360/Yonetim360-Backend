using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Twilio.Security;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Webhooks.Commands.ProcessTwilioStatusWebhook
{
    public class ProcessTwilioStatusWebhookCommandHandler : ICommandHandler<ProcessTwilioStatusWebhookCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ProcessTwilioStatusWebhookCommandHandler(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> Handle(ProcessTwilioStatusWebhookCommand request, CancellationToken cancellationToken)
        {
            ValidateSignature(request);

            var sid = GetValue(request.Parameters, "MessageSid") ?? GetValue(request.Parameters, "SmsSid");
            if (string.IsNullOrWhiteSpace(sid))
                return false;

            var message = await _context.WhatsAppMessages
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.ProviderMessageSid == sid, cancellationToken);

            if (message == null)
                return false;

            var providerStatus = GetValue(request.Parameters, "MessageStatus") ?? GetValue(request.Parameters, "SmsStatus");
            message.Status = MapStatus(providerStatus, message.Status);
            message.ProviderErrorCode = GetValue(request.Parameters, "ErrorCode");
            message.ProviderErrorMessage = GetValue(request.Parameters, "ErrorMessage");

            var now = DateTime.UtcNow;
            switch (message.Status)
            {
                case WhatsAppMessageStatus.Sent:
                    message.SentAt ??= now;
                    break;
                case WhatsAppMessageStatus.Delivered:
                    message.DeliveredAt = now;
                    break;
                case WhatsAppMessageStatus.Read:
                    message.ReadAt = now;
                    break;
                case WhatsAppMessageStatus.Failed:
                    message.FailedAt = now;
                    break;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        private void ValidateSignature(ProcessTwilioStatusWebhookCommand request)
        {
            var authToken = _configuration["Twilio:AuthToken"];
            if (string.IsNullOrWhiteSpace(authToken))
                throw new UnauthorizedAccessException("Twilio auth token is missing.");

            if (string.IsNullOrWhiteSpace(request.Signature))
                throw new UnauthorizedAccessException("Twilio signature is missing.");

            var validator = new RequestValidator(authToken);
            if (!validator.Validate(request.RequestUrl, request.Parameters, request.Signature))
                throw new UnauthorizedAccessException("Invalid Twilio signature.");
        }

        private static string? GetValue(Dictionary<string, string> values, string key)
        {
            return values.TryGetValue(key, out var value) ? value : null;
        }

        private static WhatsAppMessageStatus MapStatus(string? providerStatus, WhatsAppMessageStatus fallback)
        {
            return providerStatus?.ToLowerInvariant() switch
            {
                "queued" or "accepted" or "sending" => WhatsAppMessageStatus.Queued,
                "sent" => WhatsAppMessageStatus.Sent,
                "delivered" => WhatsAppMessageStatus.Delivered,
                "read" => WhatsAppMessageStatus.Read,
                "failed" or "undelivered" => WhatsAppMessageStatus.Failed,
                _ => fallback
            };
        }
    }
}
