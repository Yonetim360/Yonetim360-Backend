using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Queires.GetCrmSolutionRequestById
{
    public class GetCrmSolutionRequestByIdQueryValidator:AbstractValidator<GetCrmSolutionRequestByIdQuery>
    {
        public GetCrmSolutionRequestByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id cannnot be null");
        }
    }
}
