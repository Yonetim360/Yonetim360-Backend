using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class Conversation : BaseEntity, ITenantEntity
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public Customer? Customer { get; set; }
        public Guid CustomerId { get; set; }
        public ConversationType? ConversationType { get; set; }
        public string? ConversationInformation { get; set; }
        public string Subject  { get; set; }
        public DateTime StartDateTime { get; set; }       // → 29.07.2025 19:00
        public int DurationInMinutes { get; set; }
        public virtual ICollection<Representative> Representatives { get; set; }
        public ConversationStatus? ConversationStatus  { get; set; }
    }

    public enum ConversationType
    {
        Email = 1,
        Phone = 2,
        Meeting = 3,
        Chat = 4,
        Other = 5
    }
    public enum ConversationStatus
    {
        Completed = 1,   
        Cancelled = 2,   
        NoShow = 3 //görüşmeye gelinmedi
    }
}
