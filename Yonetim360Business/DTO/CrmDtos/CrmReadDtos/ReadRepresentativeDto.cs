using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Yonetim360Business.DTO.CrmDtos.CrmReadDtos
{
    public class ReadRepresentativeDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Notes { get; set; }
        public Guid UserId { get; set; }
    }
}
