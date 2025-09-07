using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CommonModule.Announcements.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommand:ICommand<bool>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Piority Piority { get; set; }
    }
}
