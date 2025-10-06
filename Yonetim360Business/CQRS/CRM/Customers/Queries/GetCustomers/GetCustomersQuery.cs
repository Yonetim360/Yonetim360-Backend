using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery:IQuery<List<ReadCustomerDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
