using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO
{
    public class CrmSolutionRequestDto
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public SolutionRequestType SolutionRequestType { get; set; }
        public string Title { get; set; }
        public Priority Piority { get; set; }
        public Module Module { get; set; }
        public string Detail { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ConversationType ConversationType { get; set; }
        public string DocumentUrl { get; set; }
    }
}
