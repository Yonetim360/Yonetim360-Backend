using Yonetim360.Entity.CRM;

namespace Yonetim360Business.Services.Abstract
{
    public interface ICrmTaskAssignmentEmailHandler
    {
        Task SendAssignmentEmailsAsync(CrmTask crmTask, IEnumerable<Representative> representatives);
    }
}
