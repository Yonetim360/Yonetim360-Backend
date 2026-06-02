using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reports.Queries.GetWhatsAppReports
{
    public class GetWhatsAppReportsQuery : IQuery<WhatsAppReportDto>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
