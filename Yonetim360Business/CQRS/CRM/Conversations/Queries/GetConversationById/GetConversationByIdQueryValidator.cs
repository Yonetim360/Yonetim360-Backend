using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversationById
{
    public class GetConversationByIdQueryValidator:AbstractValidator<GetConversationByIdQuery>
    {
        public GetConversationByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required").NotNull();
        }
    }
}
