using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO.WhatsAppDtos
{
    public class WhatsAppMessageDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerCompanyName { get; set; }
        public Guid? TemplateId { get; set; }
        public string? TemplateName { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPhoneNumber { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ContentVariablesJson { get; set; }
        public string? ProviderContentSid { get; set; }
        public string? ProviderMessageSid { get; set; }
        public string? ProviderErrorCode { get; set; }
        public string? ProviderErrorMessage { get; set; }
        public WhatsAppMessageType MessageType { get; set; }
        public WhatsAppMessageStatus Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public DateTime? NextRetryAt { get; set; }
        public int RetryCount { get; set; }
    }

    public class WhatsAppTemplateDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<string> Variables { get; set; } = new();
        public int UsageCount { get; set; }
        public string? ProviderContentSid { get; set; }
        public bool IsApproved { get; set; }
    }

    public class WhatsAppSettingsDto
    {
        public Guid Id { get; set; }
        public string? FromPhoneNumber { get; set; }
        public string? MessagingServiceSid { get; set; }
        public Guid? DefaultTemplateId { get; set; }
        public bool AutoSendEnabled { get; set; }
        public bool NotificationsEnabled { get; set; }
        public int MaxRetryAttempts { get; set; }
        public int RetryDelayMinutes { get; set; }
        public bool AllowFreeFormScheduledMessages { get; set; }
        public bool ProviderConfigured { get; set; }
        public string ConnectionStatus { get; set; } = "NotConfigured";
    }

    public class WhatsAppDashboardDto
    {
        public int TotalRemindersThisMonth { get; set; }
        public int SentMessagesThisMonth { get; set; }
        public int ActiveCustomersThisMonth { get; set; }
        public decimal SuccessRate { get; set; }
        public List<WhatsAppMessageDto> RecentReminders { get; set; } = new();
    }

    public class WhatsAppReportDto
    {
        public int TotalSent { get; set; }
        public int Delivered { get; set; }
        public int Failed { get; set; }
        public decimal SuccessRate { get; set; }
        public List<WhatsAppDailyStatDto> DailyStats { get; set; } = new();
        public List<WhatsAppStatusStatDto> StatusDistribution { get; set; } = new();
        public List<WhatsAppTemplateUsageDto> TemplateUsage { get; set; } = new();
        public List<WhatsAppHourlyStatDto> HourlyStats { get; set; } = new();
    }

    public class WhatsAppDailyStatDto
    {
        public DateTime Date { get; set; }
        public int Sent { get; set; }
        public int Delivered { get; set; }
        public int Failed { get; set; }
    }

    public class WhatsAppStatusStatDto
    {
        public WhatsAppMessageStatus Status { get; set; }
        public int Count { get; set; }
    }

    public class WhatsAppTemplateUsageDto
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public int UsageCount { get; set; }
    }

    public class WhatsAppHourlyStatDto
    {
        public int Hour { get; set; }
        public int Count { get; set; }
    }

    public class WhatsAppOutboundMessageDto
    {
        public string ToPhoneNumber { get; set; } = string.Empty;
        public string? FromPhoneNumber { get; set; }
        public string? MessagingServiceSid { get; set; }
        public string? Body { get; set; }
        public string? ContentSid { get; set; }
        public string? ContentVariablesJson { get; set; }
        public string? StatusCallbackUrl { get; set; }
    }

    public class WhatsAppSendResultDto
    {
        public string? ProviderMessageSid { get; set; }
        public WhatsAppMessageStatus Status { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
