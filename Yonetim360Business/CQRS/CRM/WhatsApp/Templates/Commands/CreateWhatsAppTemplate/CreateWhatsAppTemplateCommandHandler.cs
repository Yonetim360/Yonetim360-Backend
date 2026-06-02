using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.CreateWhatsAppTemplate
{
    public class CreateWhatsAppTemplateCommandHandler : ICommandHandler<CreateWhatsAppTemplateCommand, WhatsAppTemplateDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateWhatsAppTemplateCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WhatsAppTemplateDto> Handle(CreateWhatsAppTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = new WhatsAppTemplate
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
                Name = request.Name,
                Category = request.Category,
                Content = request.Content,
                ProviderContentSid = request.ProviderContentSid,
                IsApproved = request.IsApproved
            };
            template.SetVariables(WhatsAppMapping.ExtractVariables(request.Content));

            _context.WhatsAppTemplates.Add(template);
            await _context.SaveChangesAsync(cancellationToken);

            return WhatsAppMapping.ToDto(template);
        }
    }
}
