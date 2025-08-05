using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Representatives.Queries.GetRepresentatives
{
    public class GetRepresentativesQuery:IQuery<List<RepresentativeDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
