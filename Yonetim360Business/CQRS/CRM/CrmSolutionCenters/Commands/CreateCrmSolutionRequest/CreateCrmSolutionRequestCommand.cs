using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.CreateSolutionRequest
{
    public class CreateCrmSolutionRequestCommand:ICommand<CrmSolutionRequestDto>
    {
        public Guid CreatedBy { get; set; }
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
