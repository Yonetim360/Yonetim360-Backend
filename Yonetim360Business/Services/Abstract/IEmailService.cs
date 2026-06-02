using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO.CommonDtos;

namespace Yonetim360Business.Services.Abstract
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isbodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isbodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, List<EmailAttachment> attachments, bool isBodyHtml = true);
        Task SendMailAsync(string to, string subject, string body, List<EmailAttachment> attachments, bool isBodyHtml = true);
    }
}
