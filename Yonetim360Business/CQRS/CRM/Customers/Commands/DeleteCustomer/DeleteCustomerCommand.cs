using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
