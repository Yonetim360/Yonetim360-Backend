using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandValidator:AbstractValidator<CreateConversationCommand>
    {
        public CreateConversationCommandValidator()
        {
            RuleFor(x => x.CreatedBy)
             .NotEmpty()
             .WithMessage("UserId is required");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required");

            RuleFor(x => x.ConversationType)
                .IsInEnum()
                .WithMessage("ConversationType must be a valid enum value")
                .When(x => x.ConversationType.HasValue);

            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Subject is required")
                .MaximumLength(500)
                .WithMessage("Subject cannot exceed 500 characters");

            RuleFor(x => x.ConversationInformation)
                .MaximumLength(2000)
                .WithMessage("ConversationInformation cannot exceed 2000 characters")
                .When(x => !string.IsNullOrEmpty(x.ConversationInformation));

            RuleFor(x => x.StartDateTime)
                .NotEmpty()
                .WithMessage("StartDateTime is required");

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0)
                .WithMessage("DurationInMinutes must be greater than 0");

            RuleFor(x => x.RepresentativeIds)
                .NotEmpty()
                .WithMessage("At least one representative must be assigned");

            RuleForEach(x => x.RepresentativeIds)
                .NotEmpty()
                .WithMessage("Representative ID cannot be empty");
        }
    }
}
