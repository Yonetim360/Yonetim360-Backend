using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator:AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerDto.Id)
            .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.CustomerDto.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(100).WithMessage("Company name must not exceed 100 characters.");

            RuleFor(x => x.CustomerDto.ContactPerson)
                .NotEmpty().WithMessage("Contact person is required.")
                .MaximumLength(50).WithMessage("Contact person must not exceed 50 characters.");

            RuleFor(x => x.CustomerDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.CustomerDto.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

            RuleFor(x => x.CustomerDto.Segment)
                .IsInEnum().WithMessage("Segment value is not valid.");

            RuleFor(x => x.CustomerDto.State)
                .IsInEnum().WithMessage("State value is not valid.");

            RuleFor(x => x.CustomerDto.Address)
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

            RuleFor(x => x.CustomerDto.Note)
                .MaximumLength(500).WithMessage("Note must not exceed 500 characters.");
        }
    }
}
