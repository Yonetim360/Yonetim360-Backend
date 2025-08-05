using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO
{
    public class CustomerSupportRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public string Subject { get; set; }
        public string Explanation { get; set; }
        public Priority Priority { get; set; }
        public List<RepresentativeDto> Representatives { get; set; } = new();
    }
}
