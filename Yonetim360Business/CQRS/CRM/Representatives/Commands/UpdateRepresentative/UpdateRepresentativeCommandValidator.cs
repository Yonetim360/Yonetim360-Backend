using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.UpdateRepresentative
{
    public class UpdateRepresentativeCommandValidator:AbstractValidator<UpdateRepresentativeCommand>
    {
        public UpdateRepresentativeCommandValidator()
        {
            RuleFor(x => x.RepresentativeDto.Id).NotNull()
                .WithMessage("Representative ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("Representative ID cannot be empty.");
            RuleFor(x => x.RepresentativeDto.FirstName)
               .NotEmpty().WithMessage("Name is required.")
               .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.RepresentativeDto.LastName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.RepresentativeDto.Email).NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
            RuleFor(x => x.RepresentativeDto.PhoneNumber).NotNull()
                .WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
            RuleFor(x => x.RepresentativeDto.Notes)
                .MaximumLength(300).WithMessage("Notes cannot exceed 300 characters.");
            RuleFor(x => x.RepresentativeDto.UserId).NotNull()
                .WithMessage("User ID is required.")
                .Must(x => x != Guid.Empty).WithMessage("User ID cannot be empty.");
        }
    }
}
