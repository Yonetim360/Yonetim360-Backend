using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO
{
    public class CrmTaskDto
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public string Title { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public  RepresentativeDto Representative { get; set; }
    }
}
