using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversation
{
    public class UpdateConversationCommandValidator:AbstractValidator<UpdateConversationCommand>
    {
        public UpdateConversationCommandValidator()
        {
            RuleFor(x=>x.ConversationDto.Id).NotEmpty()
                .WithMessage("Conversation ID is required");

            RuleFor(x => x.ConversationDto.UserId)
             .NotEmpty()
             .WithMessage("UserId is required");

            RuleFor(x => x.ConversationDto.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required");

            RuleFor(x => x.ConversationDto.ConversationType)
                .IsInEnum()
                .WithMessage("ConversationType must be a valid enum value")
                .When(x => x.ConversationDto.ConversationType.HasValue);

            RuleFor(x => x.ConversationDto.Subject)
                .NotEmpty()
                .WithMessage("Subject is required")
                .MaximumLength(500)
                .WithMessage("Subject cannot exceed 500 characters");

            RuleFor(x => x.ConversationDto.ConversationInformation)
                .MaximumLength(2000)
                .WithMessage("ConversationInformation cannot exceed 2000 characters")
                .When(x => !string.IsNullOrEmpty(x.ConversationDto.ConversationInformation));

            RuleFor(x => x.ConversationDto.StartDateTime)
                .NotEmpty()
                .WithMessage("StartDateTime is required");

            RuleFor(x => x.ConversationDto.DurationInMinutes)
                .GreaterThan(0)
                .WithMessage("DurationInMinutes must be greater than 0");

            RuleFor(x => x.ConversationDto.RepresentativeIds)
    .NotEmpty()
    .WithMessage("At least one representative must be assigned");

      
        }
    }
}
