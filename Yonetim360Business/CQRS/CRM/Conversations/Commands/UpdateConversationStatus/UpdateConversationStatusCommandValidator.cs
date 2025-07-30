using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversationStatus
{
    public class UpdateConversationStatusCommandValidator:AbstractValidator<UpdateConversationStatusCommand>
    {
        public UpdateConversationStatusCommandValidator()
        {
            RuleFor(x=>x.ConversationId).NotEmpty().WithMessage("Conversation ID cannot be empty.");
            RuleFor(x => x.NewStatus)
                .NotEmpty().WithMessage("New status cannot be empty.")
                .IsInEnum().WithMessage("Invalid status value. Please provide a valid status.");
        }
    }
}
