using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Queries.GetWhatsAppMessages
{
    public class GetWhatsAppMessagesQueryHandler : IQueryHandler<GetWhatsAppMessagesQuery, List<WhatsAppMessageDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetWhatsAppMessagesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WhatsAppMessageDto>> Handle(GetWhatsAppMessagesQuery request, CancellationToken cancellationToken)
        {
            var pageSize = Math.Clamp(request.PageSize, 1, 100);
            var pageNumber = Math.Max(request.PageNumber, 1);

            var query = _context.WhatsAppMessages
                .Include(x => x.Template)
                .Include(x => x.Customer)
                .AsNoTracking()
                .Where(x =>
                    (!request.Status.HasValue || x.Status == request.Status.Value) &&
                    (!request.MessageType.HasValue || x.MessageType == request.MessageType.Value) &&
                    (!request.CustomerId.HasValue || x.CustomerId == request.CustomerId.Value));

            var messages = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return messages.Select(WhatsAppMapping.ToDto).ToList();
        }
    }
}
