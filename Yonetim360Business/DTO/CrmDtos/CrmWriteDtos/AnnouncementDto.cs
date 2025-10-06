using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Piority Piority { get; set; }

    }
}
