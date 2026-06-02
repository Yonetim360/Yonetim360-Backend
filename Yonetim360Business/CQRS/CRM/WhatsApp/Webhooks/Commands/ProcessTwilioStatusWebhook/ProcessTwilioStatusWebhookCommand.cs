using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.WhatsApp.Webhooks.Commands.ProcessTwilioStatusWebhook
{
    public class ProcessTwilioStatusWebhookCommand : ICommand<bool>
    {
        public string RequestUrl { get; set; } = string.Empty;
        public string? Signature { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }
}
