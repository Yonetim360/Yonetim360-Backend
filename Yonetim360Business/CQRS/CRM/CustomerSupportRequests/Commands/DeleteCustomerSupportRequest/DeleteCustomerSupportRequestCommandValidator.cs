using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.DeleteCustomerSupportRequest
{
    public class DeleteCustomerSupportRequestCommandValidator:AbstractValidator<DeleteCustomerSupportRequestCommand>
    {
        public DeleteCustomerSupportRequestCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id cannot null")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Id value.");
        }
    }
}
