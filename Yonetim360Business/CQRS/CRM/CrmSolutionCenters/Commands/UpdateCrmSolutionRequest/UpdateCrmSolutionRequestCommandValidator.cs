using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.UpdateSolutionRequest
{
    public class UpdateCrmSolutionRequestCommandValidator:AbstractValidator<UpdateCrmSolutionRequestCommand>
    {
        public UpdateCrmSolutionRequestCommandValidator()
        {
            RuleFor(x => x.CrmSolutionRequestDto.Id)
                .NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id cannot be null");

            RuleFor(x => x.CrmSolutionRequestDto.UserId)
          .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.CrmSolutionRequestDto.SolutionRequestType)
                .IsInEnum().WithMessage("Invalid solution request type.");

            RuleFor(x => x.CrmSolutionRequestDto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.CrmSolutionRequestDto.Piority)
                .IsInEnum().WithMessage("Invalid priority value.");

            RuleFor(x => x.CrmSolutionRequestDto.Module)
                .IsInEnum().WithMessage("Invalid module value.");

            RuleFor(x => x.CrmSolutionRequestDto.Detail)
                .NotEmpty().WithMessage("Detail is required.");

            RuleFor(x => x.CrmSolutionRequestDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.CrmSolutionRequestDto.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");
            // +905xxxxxxxxx veya 05xxxxxxxx gibi

            RuleFor(x => x.CrmSolutionRequestDto.ConversationType)
                .IsInEnum().WithMessage("Invalid conversation type.");

            RuleFor(x => x.CrmSolutionRequestDto.DocumentUrl)
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Invalid document URL format.");
        }
    }
}
