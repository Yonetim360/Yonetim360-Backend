using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand:ICommand<bool>
    {
        public required CustomerDto CustomerDto { get; set; }
    }
}
