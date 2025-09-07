using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.İK.İkAnnouncements.Commands.AssignAnnouncementToDeparments
{
    public class AssignAnnouncementToDepartmentsCommandValidator:AbstractValidator<AssignAnnouncementToDepartmentsCommand>
    {
        public AssignAnnouncementToDepartmentsCommandValidator()
        {
            RuleFor(x=>x.AnnouncementId).NotEmpty().WithMessage("AnnouncementId cannot be empty").NotNull().WithMessage("AnnouncementId cannot be null");
            RuleFor(x => x.DepartmentIds).NotEmpty().WithMessage("DepartmentIds cannot be empty").NotNull().WithMessage("DepartmentIds cannot be null");
        }
    }
}
