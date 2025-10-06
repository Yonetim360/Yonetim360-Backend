using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.CreateCustomerSupportRequest
{
    public class CreateCustomerSupportRequestCommandValidator:AbstractValidator<CreateCustomerSupportRequestCommand>
    {
        public CreateCustomerSupportRequestCommandValidator()
        {
            RuleFor(x=>x.CustomerId).NotEmpty().WithMessage("CustomerId is required.")
                .NotNull().WithMessage("CustomerId cannot null");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.Explanation).NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");
            RuleFor(x => x.Priority).NotEmpty().WithMessage("Priority is required.").IsInEnum()
                .WithMessage("Priority must be one of the defined values.");
            RuleFor(x => x.RepresentativeIds).NotEmpty().WithMessage("At least one representative is required.").NotNull().WithMessage("Representative cannot null");
            RuleFor(x => x.CreatedBy).NotEmpty().WithMessage("UserId is required.")
                .NotNull().WithMessage("UserId cannot null");


        }
    }
}
