using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand:ICommand<CustomerDto>
    {
        public Guid UserId { get; set; }
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
