using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class AnnouncementRepresentative:BaseEntity,ITenantEntity
    {
        public Guid AssignedBy { get; set; }
        public Representative Representative { get; set; }
        public Announcement Announcement { get; set; }
        public Guid AnnouncementId { get; set; }
        public Guid RepresentativeId { get; set; }
        public DateTime AssignedDate { get; set; }
        public Guid TenantId { get; set; }
    }
}
