using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class CrmTask : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public ICollection<Representative> Representative { get; set; } = new List<Representative>();
        public string Description { get; set; } 
    }

    public enum TaskCategory
    {
        General = 0,
        Call = 1,
        Meeting = 2,
        Other = 4
    }
}

