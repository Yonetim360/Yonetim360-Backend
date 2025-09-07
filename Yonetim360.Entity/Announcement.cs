using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360.Entity
{
    public class Announcement:BaseEntity,ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId  { get; set; }
        public Piority Piority { get; set; }
        public AnnouncementDepartment AnnouncementDepartment { get; set; }
        public AnnouncementRepresentative AnnouncementRepresentative { get; set; }


    }
    public enum Piority
    {
        Low=0,
        Standard = 1,   
        High =2
    }
}
