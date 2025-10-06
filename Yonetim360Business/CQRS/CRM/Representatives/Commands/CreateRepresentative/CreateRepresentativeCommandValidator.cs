using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.CreateRepresentative
{
    public class CreateRepresentativeCommandValidator:AbstractValidator<CreateRepresentativeCommand>
    {
        public CreateRepresentativeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
            RuleFor(x => x.PhoneNumber).NotNull()
                .WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
            RuleFor(x => x.Notes)
                .MaximumLength(300).WithMessage("Notes cannot exceed 300 characters.");
            RuleFor(x => x.CreatedBy).NotNull()
                .WithMessage("ApplicationUser ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("ApplicationUser ID cannot be empty.");
        }
    }
}
