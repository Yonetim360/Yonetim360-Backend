using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.UpdateSolutionRequest
{
    public class UpdateCrmSolutionRequestCommand:ICommand<bool>
    {
        public required CrmSolutionRequestDto CrmSolutionRequestDto { get; set; }
    }
}
