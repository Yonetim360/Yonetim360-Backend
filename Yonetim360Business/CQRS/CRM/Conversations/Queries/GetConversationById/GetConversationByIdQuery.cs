using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversationById
{
    public class GetConversationByIdQuery:IQuery<ReadConversationDto>
    {
        public Guid Id { get; set; }
    }
}
