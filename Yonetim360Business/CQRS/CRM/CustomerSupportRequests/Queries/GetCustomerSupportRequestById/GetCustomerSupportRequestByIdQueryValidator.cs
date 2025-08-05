using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequestById
{
    public class GetCustomerSupportRequestByIdQueryValidator:AbstractValidator<GetCustomerSupportRequestByIdQuery>
    {
        public GetCustomerSupportRequestByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be empty")
                .NotNull().WithMessage("Id cannot be null")
                .Must(id => id != Guid.Empty).WithMessage("Id must be a valid GUID");
        }
    }
}
