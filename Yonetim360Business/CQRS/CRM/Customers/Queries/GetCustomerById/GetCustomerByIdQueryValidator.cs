using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryValidator:AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator()
        {
            RuleFor(x=>x.Id).NotEmpty().WithMessage("Customer ID cannot be empty.")
                .NotNull().WithMessage("Customer ID cannot be null.")
                .Must(id => id != Guid.Empty).WithMessage("Customer ID must be a valid GUID.");
        }
    }
}
