using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class CrmTaskAssignmentEmailHandler : ICrmTaskAssignmentEmailHandler
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<CrmTaskAssignmentEmailHandler> _logger;

        public CrmTaskAssignmentEmailHandler(
            IEmailService emailService,
            ILogger<CrmTaskAssignmentEmailHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task SendAssignmentEmailsAsync(CrmTask crmTask, IEnumerable<Representative> representatives)
        {
            if (crmTask == null || representatives == null)
            {
                return;
            }

            var representativeList = representatives
                .Where(x => x != null)
                .DistinctBy(x => x.Email?.Trim().ToLowerInvariant())
                .ToList();

            if (!representativeList.Any())
            {
                _logger.LogWarning(
                    "CRM task assignment email skipped because no representative was assigned. TaskId: {TaskId}",
                    crmTask.Id);
                return;
            }

            foreach (var representative in representativeList)
            {
                if (!TryGetValidEmail(representative.Email, out var email))
                {
                    _logger.LogWarning(
                        "CRM task assignment email skipped. RepresentativeId: {RepresentativeId}, Email: {Email}",
                        representative.Id,
                        representative.Email);
                    continue;
                }

                try
                {
                    await _emailService.SendMailAsync(
                        email,
                        "Yeni gorev atamasi",
                        CreateMailBody(crmTask, representative));

                    _logger.LogInformation(
                        "CRM task assignment email sent. TaskId: {TaskId}, RepresentativeId: {RepresentativeId}, Email: {Email}",
                        crmTask.Id,
                        representative.Id,
                        email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "CRM task assignment email could not be sent. TaskId: {TaskId}, RepresentativeId: {RepresentativeId}, Email: {Email}",
                        crmTask.Id,
                        representative.Id,
                        email);
                }
            }
        }

        private static bool TryGetValidEmail(string? email, out string validEmail)
        {
            validEmail = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email.Trim());
                validEmail = mailAddress.Address;
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string CreateMailBody(CrmTask crmTask, Representative representative)
        {
            var fullName = WebUtility.HtmlEncode($"{representative.FirstName} {representative.LastName}".Trim());
            var title = WebUtility.HtmlEncode(crmTask.Title);
            var description = WebUtility.HtmlEncode(crmTask.Description);
            var startDate = crmTask.StartDate.ToString("dd.MM.yyyy HH:mm");
            var endDate = crmTask.EndDate.ToString("dd.MM.yyyy HH:mm");
            var category = WebUtility.HtmlEncode(crmTask.TaskCategory.ToString());

            return $@"
<p>Merhaba {fullName},</p>
<p>Size yeni bir CRM gorevi atandi.</p>
<p>
    <strong>Gorev:</strong> {title}<br />
    <strong>Kategori:</strong> {category}<br />
    <strong>Baslangic:</strong> {startDate}<br />
    <strong>Bitis:</strong> {endDate}
</p>
<p><strong>Aciklama:</strong><br />{description}</p>";
        }
    }
}
