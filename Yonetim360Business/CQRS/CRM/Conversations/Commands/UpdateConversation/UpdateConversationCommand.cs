using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversation
{
    public class UpdateConversationCommand:ICommand<bool>
    {
        public ConversationDto ConversationDto { get; set; }
    }
}
