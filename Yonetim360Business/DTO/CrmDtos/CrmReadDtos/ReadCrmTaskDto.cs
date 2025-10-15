using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO.CrmDtos.CrmReadDtos
{
    public class ReadCrmTaskDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string Title { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public List<ReadCrmLightRepresentativeDto> Representatives { get; set; }
    }
}
