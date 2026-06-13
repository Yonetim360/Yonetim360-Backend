using Microsoft.Extensions.Configuration;
using Twilio.Clients;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly IConfiguration _configuration;

        public WhatsAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<WhatsAppSendResultDto> SendAsync(WhatsAppOutboundMessageDto message, CancellationToken cancellationToken = default)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];

            if (string.IsNullOrWhiteSpace(accountSid) || string.IsNullOrWhiteSpace(authToken))
            {
                return new WhatsAppSendResultDto
                {
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorMessage = "Twilio account configuration is missing."
                };
            }

            try
            {
                var client = new TwilioRestClient(accountSid, authToken);
                var options = new CreateMessageOptions(new PhoneNumber(FormatWhatsAppAddress(message.ToPhoneNumber)));

                if (!string.IsNullOrWhiteSpace(message.MessagingServiceSid))
                {
                    options.MessagingServiceSid = message.MessagingServiceSid;
                }
                else
                {
                    var fromNumber = !string.IsNullOrWhiteSpace(message.FromPhoneNumber)
                        ? message.FromPhoneNumber
                        : _configuration["Twilio:DefaultFromNumber"];

                    if (string.IsNullOrWhiteSpace(fromNumber))
                    {
                        return new WhatsAppSendResultDto
                        {
                            Status = WhatsAppMessageStatus.Failed,
                            ErrorMessage = "Twilio sender phone number is missing."
                        };
                    }

                    options.From = new PhoneNumber(FormatWhatsAppAddress(fromNumber));
                }

                if (!string.IsNullOrWhiteSpace(message.ContentSid))
                {
                    options.ContentSid = message.ContentSid;
                    options.ContentVariables = message.ContentVariablesJson;
                }
                else
                {
                    options.Body = message.Body;
                }

                if (!string.IsNullOrWhiteSpace(message.StatusCallbackUrl) &&
                    Uri.TryCreate(message.StatusCallbackUrl, UriKind.Absolute, out var callbackUri))
                {
                    options.StatusCallback = callbackUri;
                }

                var response = await MessageResource.CreateAsync(options, client);

                return new WhatsAppSendResultDto
                {
                    ProviderMessageSid = response.Sid,
                    Status = MapInitialStatus(response.Status?.ToString())
                };
            }
            catch (ApiException ex)
            {
                return new WhatsAppSendResultDto
                {
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorCode = ex.Code.ToString(),
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new WhatsAppSendResultDto
                {
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<WhatsAppSendResultDto> GetMessageStatusAsync(string providerMessageSid, CancellationToken cancellationToken = default)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];

            if (string.IsNullOrWhiteSpace(accountSid) || string.IsNullOrWhiteSpace(authToken))
            {
                return new WhatsAppSendResultDto
                {
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorMessage = "Twilio account configuration is missing."
                };
            }

            if (string.IsNullOrWhiteSpace(providerMessageSid))
            {
                return new WhatsAppSendResultDto
                {
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorMessage = "Provider message SID is missing."
                };
            }

            try
            {
                var client = new TwilioRestClient(accountSid, authToken);
                var response = await MessageResource.FetchAsync(pathSid: providerMessageSid, client: client);

                return new WhatsAppSendResultDto
                {
                    ProviderMessageSid = response.Sid,
                    Status = MapInitialStatus(response.Status?.ToString()),
                    ErrorCode = response.ErrorCode?.ToString(),
                    ErrorMessage = response.ErrorMessage
                };
            }
            catch (ApiException ex)
            {
                return new WhatsAppSendResultDto
                {
                    ProviderMessageSid = providerMessageSid,
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorCode = ex.Code.ToString(),
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new WhatsAppSendResultDto
                {
                    ProviderMessageSid = providerMessageSid,
                    Status = WhatsAppMessageStatus.Failed,
                    ErrorMessage = ex.Message
                };
            }
        }

        private static string FormatWhatsAppAddress(string phoneNumber)
        {
            var trimmed = phoneNumber.Trim();
            var value = trimmed.StartsWith("whatsapp:", StringComparison.OrdinalIgnoreCase)
                ? trimmed["whatsapp:".Length..].Trim()
                : trimmed;

            return $"whatsapp:{NormalizePhoneNumber(value)}";
        }

        private static string NormalizePhoneNumber(string phoneNumber)
        {
            var trimmed = phoneNumber.Trim();
            var digits = new string(trimmed.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(digits))
                return trimmed;

            if (trimmed.StartsWith("+", StringComparison.Ordinal))
                return $"+{digits}";

            if (digits.StartsWith("00", StringComparison.Ordinal))
                return $"+{digits[2..]}";

            if (digits.Length == 10 && digits.StartsWith("5", StringComparison.Ordinal))
                return $"+90{digits}";

            return $"+{digits}";
        }

        private static WhatsAppMessageStatus MapInitialStatus(string? providerStatus)
        {
            return providerStatus?.ToLowerInvariant() switch
            {
                "accepted" or "queued" or "sending" => WhatsAppMessageStatus.Queued,
                "sent" => WhatsAppMessageStatus.Sent,
                "delivered" => WhatsAppMessageStatus.Delivered,
                "read" => WhatsAppMessageStatus.Read,
                "failed" or "undelivered" => WhatsAppMessageStatus.Failed,
                _ => WhatsAppMessageStatus.Queued
            };
        }
    }
}
