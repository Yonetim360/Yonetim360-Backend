using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommand : ICommand<bool>
    {
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public ConversationType? ConversationType { get; set; }
        public string? ConversationInformation { get; set; }
        public string Subject { get; set; }
        public DateTime StartDateTime { get; set; }
        public int DurationInMinutes { get; set; }
        public List<Guid> RepresentativeIds { get; set; } = new();
    }
}
