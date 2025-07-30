using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.UpdateOffer
{
    public class UpdateOfferCommandValidator:AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator()
        {
            RuleFor(x => x.OfferDto.Id)
                .NotEmpty().WithMessage("Teklif ID boş olamaz.")
                .NotEqual(Guid.Empty).WithMessage("Teklif ID geçersiz.");
        }
    }
}
