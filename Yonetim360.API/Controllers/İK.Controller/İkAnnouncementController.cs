using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CommonModule.Announcements.Commands.CreateAnnouncement;
using Yonetim360Business.CQRS.İK.İkAnnouncements.Commands.AssignAnnouncementToDeparments;

namespace Yonetim360.API.Controllers.İK.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class İkAnnouncementController : BaseController
    {
        [HttpPost("assign")]
        public async Task<IActionResult> AssignToDepartment(AssignAnnouncementToDepartmentsCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
