using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.BackgroundServices
{
    public class WhatsAppStatusSyncWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WhatsAppStatusSyncWorker> _logger;
        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(1);

        public WhatsAppStatusSyncWorker(IServiceScopeFactory scopeFactory, ILogger<WhatsAppStatusSyncWorker> logger)
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
                    await SyncPendingStatusesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "WhatsApp status sync worker failed.");
                }

                await Task.Delay(PollInterval, stoppingToken);
            }
        }

        private async Task SyncPendingStatusesAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();
            var now = DateTime.UtcNow;

            var messages = await context.WhatsAppMessages
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted &&
                    !string.IsNullOrWhiteSpace(x.ProviderMessageSid) &&
                    (x.Status == WhatsAppMessageStatus.Queued || x.Status == WhatsAppMessageStatus.Sent))
                .OrderBy(x => x.SentAt ?? x.CreatedAt)
                .Take(50)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                var result = await whatsAppService.GetMessageStatusAsync(message.ProviderMessageSid!, cancellationToken);
                ApplyResult(message, result, now);
            }

            if (messages.Count > 0)
                await context.SaveChangesAsync(cancellationToken);
        }

        private static void ApplyResult(WhatsAppMessage message, DTO.WhatsAppDtos.WhatsAppSendResultDto result, DateTime now)
        {
            message.ProviderErrorCode = result.ErrorCode;
            message.ProviderErrorMessage = result.ErrorMessage;

            if (!string.IsNullOrWhiteSpace(result.ProviderMessageSid))
                message.ProviderMessageSid = result.ProviderMessageSid;

            message.Status = result.Status;

            switch (message.Status)
            {
                case WhatsAppMessageStatus.Sent:
                    message.SentAt ??= now;
                    break;
                case WhatsAppMessageStatus.Delivered:
                    message.SentAt ??= now;
                    message.DeliveredAt ??= now;
                    break;
                case WhatsAppMessageStatus.Read:
                    message.SentAt ??= now;
                    message.DeliveredAt ??= now;
                    message.ReadAt ??= now;
                    break;
                case WhatsAppMessageStatus.Failed:
                    message.FailedAt ??= now;
                    break;
            }
        }
    }
}
