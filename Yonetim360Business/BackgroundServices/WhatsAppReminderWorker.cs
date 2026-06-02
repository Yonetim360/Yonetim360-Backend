using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.BackgroundServices
{
    public class WhatsAppReminderWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WhatsAppReminderWorker> _logger;
        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(1);

        public WhatsAppReminderWorker(IServiceScopeFactory scopeFactory, ILogger<WhatsAppReminderWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DispatchDueMessagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "WhatsApp reminder worker failed.");
                }

                await Task.Delay(PollInterval, stoppingToken);
            }
        }

        private async Task DispatchDueMessagesAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IWhatsAppMessageDispatcher>();
            var now = DateTime.UtcNow;

            var messages = await context.WhatsAppMessages
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted &&
                    x.MessageType == WhatsAppMessageType.Reminder &&
                    (
                        (x.Status == WhatsAppMessageStatus.Scheduled && x.ScheduledAt <= now) ||
                        (x.Status == WhatsAppMessageStatus.Failed && x.NextRetryAt <= now)
                    ))
                .OrderBy(x => x.ScheduledAt ?? x.NextRetryAt ?? x.CreatedAt)
                .Take(20)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                var settings = await context.WhatsAppSettings
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.TenantId == message.TenantId, cancellationToken);

                if (settings?.AutoSendEnabled == false)
                    continue;

                if (string.IsNullOrWhiteSpace(message.ProviderContentSid) &&
                    settings?.AllowFreeFormScheduledMessages != true)
                {
                    message.Status = WhatsAppMessageStatus.Failed;
                    message.ProviderErrorMessage = "Scheduled WhatsApp reminders require an approved Twilio ContentSid.";
                    message.FailedAt = now;
                    continue;
                }

                message.Status = WhatsAppMessageStatus.Queued;
                message.NextRetryAt = null;
                await dispatcher.DispatchAsync(message, settings, cancellationToken);

                if (message.Status == WhatsAppMessageStatus.Failed)
                {
                    message.RetryCount++;
                    var maxRetryAttempts = settings?.MaxRetryAttempts ?? 3;
                    if (message.RetryCount < maxRetryAttempts)
                    {
                        message.NextRetryAt = now.AddMinutes(settings?.RetryDelayMinutes ?? 5);
                    }
                }
            }

            if (messages.Count > 0)
                await context.SaveChangesAsync(cancellationToken);
        }
    }
}
