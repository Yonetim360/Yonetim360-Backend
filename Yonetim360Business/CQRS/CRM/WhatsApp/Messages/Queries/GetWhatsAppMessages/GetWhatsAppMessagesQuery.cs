using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Queries.GetWhatsAppMessages
{
    public class GetWhatsAppMessagesQuery : IQuery<List<WhatsAppMessageDto>>
    {
        public int PageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;
        public WhatsAppMessageStatus? Status { get; set; }
        public WhatsAppMessageType? MessageType { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
