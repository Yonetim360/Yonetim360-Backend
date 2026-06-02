using System.Text.Json;

namespace Yonetim360.Entity.CRM
{
    public class WhatsAppTemplate : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public string VariablesJson { get; set; } = "[]";
        public int UsageCount { get; set; }
        public string? ProviderContentSid { get; set; }
        public bool IsApproved { get; set; }
        public ICollection<WhatsAppMessage> Messages { get; set; } = new List<WhatsAppMessage>();

        public void SetVariables(IEnumerable<string>? variables)
        {
            VariablesJson = JsonSerializer.Serialize(variables?.Distinct().ToList() ?? new List<string>());
        }
    }
}
