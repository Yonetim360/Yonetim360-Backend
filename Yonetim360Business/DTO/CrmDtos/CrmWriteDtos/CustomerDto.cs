using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO
{
    public class CustomerDto
    {
        public Guid UpdatedBy { get; set; }
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public required Segment Segment { get; set; }
        public required State State { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
    }
}
