using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;

namespace Yonetim360Business.CQRS.CRM.WhatsApp
{
    internal static class WhatsAppMapping
    {
        public static WhatsAppMessageDto ToDto(WhatsAppMessage message)
        {
            return new WhatsAppMessageDto
            {
                Id = message.Id,
                CreatedBy = message.CreatedBy,
                CreatedAt = message.CreatedAt,
                CustomerId = message.CustomerId,
                CustomerCompanyName = message.Customer?.CompanyName,
                TemplateId = message.TemplateId,
                TemplateName = message.Template?.Name,
                RecipientName = message.RecipientName,
                RecipientPhoneNumber = message.RecipientPhoneNumber,
                Body = message.Body,
                ContentVariablesJson = message.ContentVariablesJson,
                ProviderContentSid = message.ProviderContentSid,
                ProviderMessageSid = message.ProviderMessageSid,
                ProviderErrorCode = message.ProviderErrorCode,
                ProviderErrorMessage = message.ProviderErrorMessage,
                MessageType = message.MessageType,
                Status = message.Status,
                ScheduledAt = message.ScheduledAt,
                SentAt = message.SentAt,
                DeliveredAt = message.DeliveredAt,
                ReadAt = message.ReadAt,
                FailedAt = message.FailedAt,
                NextRetryAt = message.NextRetryAt,
                RetryCount = message.RetryCount
            };
        }

        public static WhatsAppTemplateDto ToDto(WhatsAppTemplate template)
        {
            return new WhatsAppTemplateDto
            {
                Id = template.Id,
                CreatedBy = template.CreatedBy,
                CreatedAt = template.CreatedAt,
                Name = template.Name,
                Category = template.Category,
                Content = template.Content,
                Variables = DeserializeVariables(template.VariablesJson),
                UsageCount = template.UsageCount,
                ProviderContentSid = template.ProviderContentSid,
                IsApproved = template.IsApproved
            };
        }

        public static WhatsAppSettingsDto ToDto(WhatsAppSettings settings, IConfiguration configuration)
        {
            var providerConfigured =
                !string.IsNullOrWhiteSpace(configuration["Twilio:AccountSid"]) &&
                !string.IsNullOrWhiteSpace(configuration["Twilio:AuthToken"]) &&
                (!string.IsNullOrWhiteSpace(settings.FromPhoneNumber) ||
                 !string.IsNullOrWhiteSpace(settings.MessagingServiceSid) ||
                 !string.IsNullOrWhiteSpace(configuration["Twilio:DefaultFromNumber"]));
            var callbackConfigured =
                !string.IsNullOrWhiteSpace(configuration["Twilio:StatusCallbackUrl"]) ||
                !string.IsNullOrWhiteSpace(configuration["Twilio:StatusCallbackBaseUrl"]);

            return new WhatsAppSettingsDto
            {
                Id = settings.Id,
                FromPhoneNumber = settings.FromPhoneNumber,
                MessagingServiceSid = settings.MessagingServiceSid,
                DefaultTemplateId = settings.DefaultTemplateId,
                AutoSendEnabled = settings.AutoSendEnabled,
                NotificationsEnabled = settings.NotificationsEnabled,
                MaxRetryAttempts = settings.MaxRetryAttempts,
                RetryDelayMinutes = settings.RetryDelayMinutes,
                AllowFreeFormScheduledMessages = settings.AllowFreeFormScheduledMessages,
                ProviderConfigured = providerConfigured,
                ConnectionStatus = !providerConfigured
                    ? "NotConfigured"
                    : callbackConfigured
                        ? "Configured"
                        : "ConfiguredWithoutStatusCallback"
            };
        }

        public static List<string> ExtractVariables(string content)
        {
            return Regex.Matches(content ?? string.Empty, "{([^}]+)}")
                .Select(x => x.Groups[1].Value.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public static List<string> DeserializeVariables(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static WhatsAppSettings CreateDefaultSettings(Guid createdBy)
        {
            return new WhatsAppSettings
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                AutoSendEnabled = true,
                NotificationsEnabled = true,
                MaxRetryAttempts = 3,
                RetryDelayMinutes = 5,
                AllowFreeFormScheduledMessages = false
            };
        }
    }
}
