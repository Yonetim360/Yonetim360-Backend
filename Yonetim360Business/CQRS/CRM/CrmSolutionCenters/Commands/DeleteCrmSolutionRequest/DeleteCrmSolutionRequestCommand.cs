using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.DeleteSolutionRequest
{
    public class DeleteCrmSolutionRequestCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
