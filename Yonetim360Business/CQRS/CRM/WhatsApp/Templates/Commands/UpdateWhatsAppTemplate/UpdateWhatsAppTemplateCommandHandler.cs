using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.DTO.WhatsAppDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.UpdateWhatsAppTemplate
{
    public class UpdateWhatsAppTemplateCommandHandler : ICommandHandler<UpdateWhatsAppTemplateCommand, WhatsAppTemplateDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateWhatsAppTemplateCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WhatsAppTemplateDto> Handle(UpdateWhatsAppTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await _context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new InvalidDataException("WhatsApp template not found.");

            template.UpdatedBy = request.UpdatedBy;
            template.Name = request.Name;
            template.Category = request.Category;
            template.Content = request.Content;
            template.ProviderContentSid = request.ProviderContentSid;
            template.IsApproved = request.IsApproved;
            template.SetVariables(WhatsAppMapping.ExtractVariables(request.Content));

            await _context.SaveChangesAsync(cancellationToken);
            return WhatsAppMapping.ToDto(template);
        }
    }
}
