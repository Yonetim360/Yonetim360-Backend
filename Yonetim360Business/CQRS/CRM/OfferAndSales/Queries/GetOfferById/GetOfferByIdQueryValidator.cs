using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Queries.GetOfferById
{
    public class GetOfferByIdQueryValidator:AbstractValidator<GetOfferByIdQuery>
    {
        public GetOfferByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Teklif ID boş olamaz.");
        }
    }
}
