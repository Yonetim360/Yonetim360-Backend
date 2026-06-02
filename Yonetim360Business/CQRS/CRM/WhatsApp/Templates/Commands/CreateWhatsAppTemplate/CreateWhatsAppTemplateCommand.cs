using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.CreateWhatsAppTemplate
{
    public class CreateWhatsAppTemplateCommand : ICommand<WhatsAppTemplateDto>
    {
        public Guid CreatedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ProviderContentSid { get; set; }
        public bool IsApproved { get; set; }
    }
}
