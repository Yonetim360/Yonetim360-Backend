using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversations
{
    public class GetConversationQuery:IQuery<List<ConversationDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public Guid? CustomerId { get; set; }
        public ConversationStatus? ConversationStatus { get; set; }
    }
}
