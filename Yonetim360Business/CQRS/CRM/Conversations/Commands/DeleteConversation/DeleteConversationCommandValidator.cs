using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.DeleteConversation
{
    public class DeleteConversationCommandValidator:AbstractValidator<DeleteConversationCommand>
    {
        public DeleteConversationCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty().WithMessage("Id boş olamaz.")
                .NotNull().WithMessage("Id boş olamaz.");
        }
    }
}
