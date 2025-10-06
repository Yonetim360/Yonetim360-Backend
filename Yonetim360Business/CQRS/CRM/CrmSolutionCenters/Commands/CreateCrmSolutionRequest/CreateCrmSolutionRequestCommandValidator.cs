using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.CreateSolutionRequest
{
    public class CreateCrmSolutionRequestCommandValidator:AbstractValidator<CreateCrmSolutionRequestCommand>
    {
        public CreateCrmSolutionRequestCommandValidator()
        {
            RuleFor(x => x.CreatedBy)
           .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.SolutionRequestType)
                .IsInEnum().WithMessage("Invalid solution request type.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Piority)
                .IsInEnum().WithMessage("Invalid priority value.");

            RuleFor(x => x.Module)
                .IsInEnum().WithMessage("Invalid module value.");

            RuleFor(x => x.Detail)
                .NotEmpty().WithMessage("Detail is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");
            // +905xxxxxxxxx veya 05xxxxxxxx gibi

            RuleFor(x => x.ConversationType)
                .IsInEnum().WithMessage("Invalid conversation type.");

        }
    }
}
