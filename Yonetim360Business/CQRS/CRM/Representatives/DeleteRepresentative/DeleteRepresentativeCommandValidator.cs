using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Representatives.DeleteRepresentative
{
    public class DeleteRepresentativeCommandValidator:AbstractValidator<DeleteRepresentativeCommand>
    {
        public DeleteRepresentativeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
