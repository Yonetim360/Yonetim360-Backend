using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360.Entity.İK;

namespace Yonetim360.Entity
{
    public class AnnouncementDepartment:BaseEntity,ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Department Department { get; set; }
        public Announcement Announcement { get; set; }

        public Guid DepartmentId { get; set; }
        public Guid AnnouncementId { get; set; }
        public DateTime AssignedDate { get; set; }
        public Guid AssignedBy { get; set; }
    }
}
