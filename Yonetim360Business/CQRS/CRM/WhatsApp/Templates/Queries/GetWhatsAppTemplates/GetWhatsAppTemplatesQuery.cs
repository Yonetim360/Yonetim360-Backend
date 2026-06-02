using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Queries.GetWhatsAppTemplates
{
    public class GetWhatsAppTemplatesQuery : IQuery<List<WhatsAppTemplateDto>>
    {
        public int PageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;
        public string? Category { get; set; }
    }
}
