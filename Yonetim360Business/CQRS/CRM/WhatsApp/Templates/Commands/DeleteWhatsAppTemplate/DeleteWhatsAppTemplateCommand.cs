using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.DeleteWhatsAppTemplate
{
    public class DeleteWhatsAppTemplateCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
