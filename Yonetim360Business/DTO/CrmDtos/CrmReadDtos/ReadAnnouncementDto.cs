using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity;

namespace Yonetim360Business.DTO.CrmDtos.CrmReadDtos
{
    public class ReadAnnouncementDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Piority Piority { get; set; }
    }
}
