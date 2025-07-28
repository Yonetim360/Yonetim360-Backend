using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryValidator:AbstractValidator<GetCustomersQuery>
    {
        public GetCustomersQueryValidator()
        {
            RuleFor(x=>x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size must be less than or equal to 100.");
            RuleFor(x => x.PageNumber).NotNull().WithMessage("Page number cannot be null.")
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");
        }
    }
}
