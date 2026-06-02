using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.DeleteWhatsAppTemplate
{
    public class DeleteWhatsAppTemplateCommandHandler : ICommandHandler<DeleteWhatsAppTemplateCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteWhatsAppTemplateCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteWhatsAppTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await _context.WhatsAppTemplates.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                ?? throw new InvalidDataException("WhatsApp template not found.");

            _context.WhatsAppTemplates.Remove(template);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
