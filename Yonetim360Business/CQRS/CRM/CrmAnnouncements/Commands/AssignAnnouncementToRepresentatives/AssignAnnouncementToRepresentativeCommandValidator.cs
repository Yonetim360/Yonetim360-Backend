using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Commands.AssignAnnouncementToRepresentatives
{
    public class AssignAnnouncementToRepresentativeCommandValidator:AbstractValidator<AssignAnnouncementToRepresentativeCommand>
    {
        public AssignAnnouncementToRepresentativeCommandValidator()
        {
            RuleFor(x=>x.AnnouncementId).NotNull().WithMessage("AnnouncementId is required.").NotEmpty().WithMessage("AnnouncementId cannot be empty");
            RuleFor(x => x.RepresentativeIds).NotNull().WithMessage("RepresentativeIds is required.").NotEmpty().WithMessage("RepresentativeIds cannot be empty");
        }
    }
}
