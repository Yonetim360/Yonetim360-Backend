namespace Yonetim360.Entity.CRM
{
    public class WhatsAppMessage : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid? TemplateId { get; set; }
        public WhatsAppTemplate? Template { get; set; }
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

    public enum WhatsAppMessageType
    {
        Manual = 1,
        Reminder = 2
    }

    public enum WhatsAppMessageStatus
    {
        Scheduled = 1,
        Queued = 2,
        Sent = 3,
        Delivered = 4,
        Read = 5,
        Failed = 6,
        Cancelled = 7
    }
}
