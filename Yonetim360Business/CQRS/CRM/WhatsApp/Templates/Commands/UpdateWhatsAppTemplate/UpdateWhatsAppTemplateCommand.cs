using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.UpdateWhatsAppTemplate
{
    public class UpdateWhatsAppTemplateCommand : ICommand<WhatsAppTemplateDto>
    {
        public Guid Id { get; set; }
        public Guid UpdatedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ProviderContentSid { get; set; }
        public bool IsApproved { get; set; }
    }
}
