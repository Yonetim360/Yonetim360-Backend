using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Queries.GetWhatsAppTemplates
{
    public class GetWhatsAppTemplatesQueryHandler : IQueryHandler<GetWhatsAppTemplatesQuery, List<WhatsAppTemplateDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetWhatsAppTemplatesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WhatsAppTemplateDto>> Handle(GetWhatsAppTemplatesQuery request, CancellationToken cancellationToken)
        {
            var pageSize = Math.Clamp(request.PageSize, 1, 100);
            var pageNumber = Math.Max(request.PageNumber, 1);

            var templates = await _context.WhatsAppTemplates
                .AsNoTracking()
                .Where(x => string.IsNullOrWhiteSpace(request.Category) || x.Category == request.Category)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return templates.Select(WhatsAppMapping.ToDto).ToList();
        }
    }
}
