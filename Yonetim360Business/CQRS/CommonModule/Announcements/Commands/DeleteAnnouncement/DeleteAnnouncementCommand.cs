using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CommonModule.Announcements.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementCommand:ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
