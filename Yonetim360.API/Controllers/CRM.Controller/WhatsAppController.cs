using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yonetim360.Entity.CRM;
using Yonetim360Business.CQRS.CRM.WhatsApp.Dashboard.Queries.GetWhatsAppDashboard;
using Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Commands.SendWhatsAppMessage;
using Yonetim360Business.CQRS.CRM.WhatsApp.Messages.Queries.GetWhatsAppMessages;
using Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.CreateWhatsAppReminder;
using Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.DeleteWhatsAppReminder;
using Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Commands.UpdateWhatsAppReminder;
using Yonetim360Business.CQRS.CRM.WhatsApp.Reminders.Queries.GetWhatsAppReminders;
using Yonetim360Business.CQRS.CRM.WhatsApp.Reports.Queries.GetWhatsAppReports;
using Yonetim360Business.CQRS.CRM.WhatsApp.Settings.Commands.UpdateWhatsAppSettings;
using Yonetim360Business.CQRS.CRM.WhatsApp.Settings.Queries.GetWhatsAppSettings;
using Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.CreateWhatsAppTemplate;
using Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.DeleteWhatsAppTemplate;
using Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Commands.UpdateWhatsAppTemplate;
using Yonetim360Business.CQRS.CRM.WhatsApp.Templates.Queries.GetWhatsAppTemplates;
using Yonetim360Business.CQRS.CRM.WhatsApp.Webhooks.Commands.ProcessTwilioStatusWebhook;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : BaseController
    {
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            return Ok(await Mediator.Send(new GetWhatsAppDashboardQuery()));
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages(
            [FromQuery] WhatsAppMessageStatus? status,
            [FromQuery] WhatsAppMessageType? messageType,
            [FromQuery] Guid? customerId,
            [FromQuery] int pageSize = 50,
            [FromQuery] int pageNumber = 1)
        {
            var result = await Mediator.Send(new GetWhatsAppMessagesQuery
            {
                Status = status,
                MessageType = messageType,
                CustomerId = customerId,
                PageSize = pageSize,
                PageNumber = pageNumber
            });

            return Ok(result);
        }

        [HttpPost("messages/send")]
        public async Task<IActionResult> SendMessage(SendWhatsAppMessageCommand command)
        {
            command.CreatedBy = GetCurrentUserId(command.CreatedBy);
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("reminders")]
        public async Task<IActionResult> GetReminders(
            [FromQuery] WhatsAppMessageStatus? status,
            [FromQuery] int pageSize = 50,
            [FromQuery] int pageNumber = 1)
        {
            var result = await Mediator.Send(new GetWhatsAppRemindersQuery
            {
                Status = status,
                PageSize = pageSize,
                PageNumber = pageNumber
            });

            return Ok(result);
        }

        [HttpPost("reminders")]
        public async Task<IActionResult> CreateReminder(CreateWhatsAppReminderCommand command)
        {
            command.CreatedBy = GetCurrentUserId(command.CreatedBy);
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("reminders/{id}")]
        public async Task<IActionResult> UpdateReminder(Guid id, UpdateWhatsAppReminderCommand command)
        {
            command.Id = id;
            command.UpdatedBy = GetCurrentUserId(command.UpdatedBy);
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("reminders/{id}")]
        public async Task<IActionResult> DeleteReminder(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteWhatsAppReminderCommand { Id = id }));
        }

        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates(
            [FromQuery] string? category,
            [FromQuery] int pageSize = 50,
            [FromQuery] int pageNumber = 1)
        {
            return Ok(await Mediator.Send(new GetWhatsAppTemplatesQuery
            {
                Category = category,
                PageSize = pageSize,
                PageNumber = pageNumber
            }));
        }

        [HttpPost("templates")]
        public async Task<IActionResult> CreateTemplate(CreateWhatsAppTemplateCommand command)
        {
            command.CreatedBy = GetCurrentUserId(command.CreatedBy);
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("templates/{id}")]
        public async Task<IActionResult> UpdateTemplate(Guid id, UpdateWhatsAppTemplateCommand command)
        {
            command.Id = id;
            command.UpdatedBy = GetCurrentUserId(command.UpdatedBy);
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("templates/{id}")]
        public async Task<IActionResult> DeleteTemplate(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteWhatsAppTemplateCommand { Id = id }));
        }

        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            return Ok(await Mediator.Send(new GetWhatsAppSettingsQuery()));
        }

        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings(UpdateWhatsAppSettingsCommand command)
        {
            command.UpdatedBy = GetCurrentUserId(command.UpdatedBy);
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReports([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return Ok(await Mediator.Send(new GetWhatsAppReportsQuery
            {
                StartDate = startDate,
                EndDate = endDate
            }));
        }

        [AllowAnonymous]
        [HttpPost("webhooks/twilio/status")]
        public async Task<IActionResult> TwilioStatusWebhook()
        {
            var form = await Request.ReadFormAsync();
            var parameters = form.ToDictionary(x => x.Key, x => x.Value.ToString(), StringComparer.OrdinalIgnoreCase);
            var requestUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            try
            {
                var result = await Mediator.Send(new ProcessTwilioStatusWebhookCommand
                {
                    RequestUrl = requestUrl,
                    Signature = Request.Headers["X-Twilio-Signature"].FirstOrDefault(),
                    Parameters = parameters
                });

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        private Guid GetCurrentUserId(Guid fallback)
        {
            if (fallback != Guid.Empty)
                return fallback;

            var claimValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                User.FindFirstValue("sub");

            return Guid.TryParse(claimValue, out var userId)
                ? userId
                : throw new UnauthorizedAccessException("Kullanici bilgisi bulunamadi.");
        }
    }
}
