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
        public TaskCategory TaskCategory { get; set; }
        public DateTime Time { get; set; }
        public Representative Representative { get; set; }
        public Guid RepresentativeId { get; set; }
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

