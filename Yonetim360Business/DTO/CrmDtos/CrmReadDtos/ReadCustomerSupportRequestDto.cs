using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO.CrmReadDtos
{
    public class ReadCustomerSupportRequestDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerCompanyName { get; set; }
        public string Subject { get; set; }
        public string Explanation { get; set; }
        public Priority Priority { get; set; }
        public List<RepresentativeDto> Representatives { get; set; } = new();
        public SupportRequestStatus SupportRequestStatus { get; set; }
    }
}
