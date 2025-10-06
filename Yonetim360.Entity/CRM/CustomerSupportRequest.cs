using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class CustomerSupportRequest:BaseEntity,ITenantEntity
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; } 
        public string Subject { get; set; }
        public string Explanation { get; set; }
        public Priority Priority { get; set; }
        public virtual ICollection<Representative> Representatives { get; set; } 
        public SupportRequestStatus SupportRequestStatus { get; set; } = SupportRequestStatus.Pending;

    }
    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critic = 4
    }

    public enum SupportRequestStatus
    {
       Pending=0,
       Completed=1
    }
}
