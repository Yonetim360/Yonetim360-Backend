using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncementByRepresentativeId
{
    public class GetAnnouncementsByRepresentativeIdQueryValidator:AbstractValidator<GetAnnouncementsByRepresentativeIdQuery>
    {
        public GetAnnouncementsByRepresentativeIdQueryValidator()
        {
            RuleFor(x => x.RepresentativeId).NotEmpty().WithMessage("RepresentativeId is required.").NotNull().WithMessage("RepresentativeId cannot be null");
        }
    }
}
