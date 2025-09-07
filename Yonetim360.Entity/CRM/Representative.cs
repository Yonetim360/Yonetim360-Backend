using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class Representative:BaseEntity,ITenantEntity
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public CrmDepartment CrmDepartment { get; set; }
        public string Notes { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
        public ICollection<CustomerSupportRequest> CustomerSupportRequests { get; set; }
        public ICollection<CrmTask> CrmTasks { get; set; }

    }
    public enum CrmDepartment
    {
        Sales,
        Support,
        Marketing,
        Management
    }
}
