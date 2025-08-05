using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.UpdateCustomerSupportRequest
{
    public class UpdateCustomerSupportRequestCommand:ICommand<bool>
    {
        public required CustomerSupportRequestDto CustomerSupportRequestDto { get; set; }
    }
}
