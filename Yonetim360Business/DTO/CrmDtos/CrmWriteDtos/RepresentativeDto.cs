using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO
{
    public class RepresentativeDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid UpdatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
    }
}
