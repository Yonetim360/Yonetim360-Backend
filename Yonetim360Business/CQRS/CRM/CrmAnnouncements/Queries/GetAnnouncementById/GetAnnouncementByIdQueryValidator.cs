using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncementById
{
    public class GetAnnouncementByIdQueryValidator:AbstractValidator<GetAnnouncementByIdQuery>
    {
        public GetAnnouncementByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("AnnouncementId cannot be empty").NotNull().WithMessage("AnnouncementId cannot be null");
        }
    }
}
