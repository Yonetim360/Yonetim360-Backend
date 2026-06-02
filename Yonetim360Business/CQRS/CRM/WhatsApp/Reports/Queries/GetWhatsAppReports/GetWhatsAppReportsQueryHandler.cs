using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reports.Queries.GetWhatsAppReports
{
    public class GetWhatsAppReportsQueryHandler : IQueryHandler<GetWhatsAppReportsQuery, WhatsAppReportDto>
    {
        private readonly ApplicationDbContext _context;

        public GetWhatsAppReportsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WhatsAppReportDto> Handle(GetWhatsAppReportsQuery request, CancellationToken cancellationToken)
        {
            var end = request.EndDate?.Date.AddDays(1) ?? DateTime.UtcNow.Date.AddDays(1);
            var start = request.StartDate?.Date ?? end.AddDays(-7);

            var messages = await _context.WhatsAppMessages
                .Include(x => x.Template)
                .AsNoTracking()
                .Where(x => x.CreatedAt >= start && x.CreatedAt < end)
                .ToListAsync(cancellationToken);

            var delivered = messages.Count(x => x.Status == WhatsAppMessageStatus.Delivered || x.Status == WhatsAppMessageStatus.Read);
            var failed = messages.Count(x => x.Status == WhatsAppMessageStatus.Failed);
            var totalCompleted = delivered + failed;

            return new WhatsAppReportDto
            {
                TotalSent = messages.Count(x => x.Status != WhatsAppMessageStatus.Scheduled && x.Status != WhatsAppMessageStatus.Cancelled),
                Delivered = delivered,
                Failed = failed,
                SuccessRate = totalCompleted == 0 ? 0 : Math.Round((decimal)delivered * 100 / totalCompleted, 2),
                DailyStats = messages
                    .GroupBy(x => x.CreatedAt.Date)
                    .OrderBy(x => x.Key)
                    .Select(x => new WhatsAppDailyStatDto
                    {
                        Date = x.Key,
                        Sent = x.Count(m => m.Status != WhatsAppMessageStatus.Scheduled && m.Status != WhatsAppMessageStatus.Cancelled),
                        Delivered = x.Count(m => m.Status == WhatsAppMessageStatus.Delivered || m.Status == WhatsAppMessageStatus.Read),
                        Failed = x.Count(m => m.Status == WhatsAppMessageStatus.Failed)
                    })
                    .ToList(),
                StatusDistribution = messages
                    .GroupBy(x => x.Status)
                    .Select(x => new WhatsAppStatusStatDto { Status = x.Key, Count = x.Count() })
                    .ToList(),
                TemplateUsage = messages
                    .Where(x => x.TemplateId.HasValue && x.Template != null)
                    .GroupBy(x => new { x.TemplateId, x.Template!.Name })
                    .Select(x => new WhatsAppTemplateUsageDto
                    {
                        TemplateId = x.Key.TemplateId!.Value,
                        TemplateName = x.Key.Name,
                        UsageCount = x.Count()
                    })
                    .OrderByDescending(x => x.UsageCount)
                    .ToList(),
                HourlyStats = messages
                    .GroupBy(x => x.CreatedAt.Hour)
                    .OrderBy(x => x.Key)
                    .Select(x => new WhatsAppHourlyStatDto { Hour = x.Key, Count = x.Count() })
                    .ToList()
            };
        }
    }
}
