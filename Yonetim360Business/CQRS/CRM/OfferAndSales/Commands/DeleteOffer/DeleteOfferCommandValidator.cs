using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.DeleteOffer
{
    public class DeleteOfferCommandValidator:AbstractValidator<DeleteOfferCommand>
    {
        public DeleteOfferCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty().WithMessage("Teklif Id boş olamaz.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı Id boş olamaz.");
        }
    }
}
