using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CommonModule.Announcements.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementCommandValidator:AbstractValidator<DeleteAnnouncementCommand>
    {
        public DeleteAnnouncementCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Duyuru Id boş olamaz");
        }
    }
}
