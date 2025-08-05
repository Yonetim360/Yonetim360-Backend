using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.Representatives.Queries.GetRepresentativeById
{
    public class GetRepresentativeByIdQueryValidator:AbstractValidator<GetRepresentativeByIdQuery>
    {
        public GetRepresentativeByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull()
                .WithMessage("Id is rquired.");

        }
    }
}
