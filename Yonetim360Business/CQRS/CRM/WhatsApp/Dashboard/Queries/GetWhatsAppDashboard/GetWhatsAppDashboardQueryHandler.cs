using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Dashboard.Queries.GetWhatsAppDashboard
{
    public class GetWhatsAppDashboardQueryHandler : IQueryHandler<GetWhatsAppDashboardQuery, WhatsAppDashboardDto>
    {
        private readonly ApplicationDbContext _context;

        public GetWhatsAppDashboardQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WhatsAppDashboardDto> Handle(GetWhatsAppDashboardQuery request, CancellationToken cancellationToken)
        {
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var monthMessages = _context.WhatsAppMessages.AsNoTracking()
                .Where(x => x.CreatedAt >= monthStart);

            var sentCount = await monthMessages.CountAsync(x =>
                x.Status == WhatsAppMessageStatus.Queued ||
                x.Status == WhatsAppMessageStatus.Sent ||
                x.Status == WhatsAppMessageStatus.Delivered ||
                x.Status == WhatsAppMessageStatus.Read, cancellationToken);

            var successfulCount = await monthMessages.CountAsync(x =>
                x.Status == WhatsAppMessageStatus.Delivered ||
                x.Status == WhatsAppMessageStatus.Read, cancellationToken);

            var failedCount = await monthMessages.CountAsync(x => x.Status == WhatsAppMessageStatus.Failed, cancellationToken);
            var completedTotal = successfulCount + failedCount;

            var recentReminders = await _context.WhatsAppMessages
                .Include(x => x.Template)
                .Include(x => x.Customer)
                .AsNoTracking()
                .Where(x => x.MessageType == WhatsAppMessageType.Reminder)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToListAsync(cancellationToken);

            return new WhatsAppDashboardDto
            {
                TotalRemindersThisMonth = await monthMessages.CountAsync(x => x.MessageType == WhatsAppMessageType.Reminder, cancellationToken),
                SentMessagesThisMonth = sentCount,
                ActiveCustomersThisMonth = await monthMessages
                    .Where(x => x.CustomerId.HasValue)
                    .Select(x => x.CustomerId)
                    .Distinct()
                    .CountAsync(cancellationToken),
                SuccessRate = completedTotal == 0 ? 0 : Math.Round((decimal)successfulCount * 100 / completedTotal, 2),
                RecentReminders = recentReminders.Select(WhatsAppMapping.ToDto).ToList()
            };
        }
    }
}
