using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.İK.İkAnnouncements.Commands.AssignAnnouncementToDeparments
{
    public class AssignAnnouncementToDepartmentsCommand:ICommand<bool>
    {
        public Guid AnnouncementId { get; set; }
        public List<Guid> DepartmentIds { get; set; }
    }
}
