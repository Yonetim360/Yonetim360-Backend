using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Queires.GetCrmSolutionRequests
{
    public class GetCrmSolutionRequestsQueryValidator:AbstractValidator<GetCrmSolutionRequestsQuery>
    {
        public GetCrmSolutionRequestsQueryValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize should be greater than 0");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber should be greater than 0");
        }
    }
}
