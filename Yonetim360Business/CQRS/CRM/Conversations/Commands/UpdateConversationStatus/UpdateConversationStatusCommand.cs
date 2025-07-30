using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversationStatus
{
    public class UpdateConversationStatusCommand:ICommand<bool>
    {
        public Guid ConversationId { get; set; }
        public ConversationStatus NewStatus { get; set; }
    }
}
