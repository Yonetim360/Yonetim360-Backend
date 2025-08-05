using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequests
{
    public class GetCustomerSupportRequestsQuereyValidator:AbstractValidator<GetCustomerSupportRequestsQuery>
    {
        public GetCustomerSupportRequestsQuereyValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        }
    }
}
