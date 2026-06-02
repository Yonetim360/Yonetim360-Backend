using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Queries.GetWhatsAppReminders
{
    public class GetWhatsAppRemindersQueryHandler : IQueryHandler<GetWhatsAppRemindersQuery, List<WhatsAppMessageDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetWhatsAppRemindersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WhatsAppMessageDto>> Handle(GetWhatsAppRemindersQuery request, CancellationToken cancellationToken)
        {
            var pageSize = Math.Clamp(request.PageSize, 1, 100);
            var pageNumber = Math.Max(request.PageNumber, 1);

            var reminders = await _context.WhatsAppMessages
                .Include(x => x.Template)
                .Include(x => x.Customer)
                .AsNoTracking()
                .Where(x => x.MessageType == WhatsAppMessageType.Reminder &&
                    (!request.Status.HasValue || x.Status == request.Status.Value))
                .OrderByDescending(x => x.ScheduledAt ?? x.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return reminders.Select(WhatsAppMapping.ToDto).ToList();
        }
    }
}
