using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Commands.AssignAnnouncementToRepresentatives
{
    public class AssignAnnouncementToRepresentativeCommand:ICommand<bool>
    {
        public Guid AnnouncementId { get; set; }
        public List<Guid> RepresentativeIds { get; set; }
        public Guid AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }

    }
}
