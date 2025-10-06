using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.CreateCustomerSupportRequest
{
    public class CreateCustomerSupportRequestCommand:ICommand<CustomerSupportRequestDto>
    {
        public Guid CreatedBy { get; set; }
        public Guid CustomerId { get; set; }
        public string Subject { get; set; }
        public string Explanation { get; set; }
        public Priority Priority { get; set; }
        public List<Guid> RepresentativeIds { get; set; } = new();
    }
}
