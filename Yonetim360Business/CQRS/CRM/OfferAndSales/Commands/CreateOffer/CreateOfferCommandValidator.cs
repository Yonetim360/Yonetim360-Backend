using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.CreateOffer
{
    public class CreateOfferCommandValidator:AbstractValidator<CreateOfferCommand>
    {
        public CreateOfferCommandValidator()
        {
            RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("User must be selected.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Offer title cannot be empty.")
                .MaximumLength(100).WithMessage("Title can be up to 100 characters long.");

            RuleFor(x => x.RepresentativeId)
                .NotEmpty().WithMessage("A representative must be selected.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer must be selected.");

            RuleFor(x => x.OfferStatus)
                .IsInEnum().WithMessage("Invalid offer status.");

            RuleFor(x => x.ServiceExplanation)
                .NotEmpty().WithMessage("Service explanation cannot be empty.")
                .MaximumLength(1000).WithMessage("Service explanation can be up to 1000 characters.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Amount cannot be empty.")
                .Must(amount =>
                    decimal.TryParse(
                        amount?.Replace("₺", "")
                               .Replace("$", "")
                               .Replace(",", "")
                               .Replace(".", ","),
                        out _)
                ).WithMessage("Amount must be a valid number.");

            RuleFor(x => x.DiscountType)
                .IsInEnum().WithMessage("Invalid discount type.");

            RuleFor(x => x.DiscountValue)
                .GreaterThanOrEqualTo(0).WithMessage("Discount value cannot be negative.");

            RuleFor(x => x.FinalAmount)
                .GreaterThanOrEqualTo(0).When(x => x.FinalAmount.HasValue)
                .WithMessage("Final amount cannot be negative.");

            RuleFor(x => x.ValidityDate)
                .GreaterThan(DateTime.Now).WithMessage("Validity date must be in the future.");

            RuleFor(x => x.Note)
                .MaximumLength(1000).WithMessage("Note can be up to 1000 characters.");

            RuleFor(x => x.Currency)
                .IsInEnum().WithMessage("Invalid currency.");
        }
    }
}
