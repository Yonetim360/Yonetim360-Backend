using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator:AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(100).WithMessage("Company name must not exceed 100 characters.");

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage("Contact person is required.")
                .MaximumLength(50).WithMessage("Contact person must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

            RuleFor(x => x.Segment)
                .IsInEnum().WithMessage("Segment value is not valid.");

            RuleFor(x => x.State)
                .IsInEnum().WithMessage("State value is not valid.");

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Note must not exceed 500 characters.");
        }
    }
}
