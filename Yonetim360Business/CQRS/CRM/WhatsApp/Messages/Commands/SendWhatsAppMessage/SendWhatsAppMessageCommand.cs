using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Commands.SendWhatsAppMessage
{
    public class SendWhatsAppMessageCommand : ICommand<WhatsAppMessageDto>
    {
        public Guid CreatedBy { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? TemplateId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientPhoneNumber { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ContentVariablesJson { get; set; }
    }
}
