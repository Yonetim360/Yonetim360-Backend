using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequestById
{
    public class GetCustomerSupportRequestByIdQuery:IQuery<ReadCustomerSupportRequestDto>
    {
        public Guid Id { get; set; }

    }
}
