using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CommonModule.Announcements.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommandValidator:AbstractValidator<CreateAnnouncementCommand>
    {
        public CreateAnnouncementCommandValidator()
        {
            RuleFor(x=>x.UserId).NotNull().WithMessage("UserId cannot be null")
                .NotEmpty().WithMessage("UserId cannot be empty");

            RuleFor(x=>x.Title).NotNull().WithMessage("Title cannot be null")
                .NotEmpty().WithMessage("Title cannot be empty").MaximumLength(500)
                .WithMessage("Title cannot be longer than 500 characters");

            RuleFor(x=>x.Content).NotNull().WithMessage("Content cannot be null")
                .NotEmpty().WithMessage("Content cannot be empty").MaximumLength(4000)
                .WithMessage("Content cannot be longer than 4000 characters");

            RuleFor(x => x.Piority).IsInEnum().WithMessage("Piority must be a valid enum value");

          

        }
    }
}
