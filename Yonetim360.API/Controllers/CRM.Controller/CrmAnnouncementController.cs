using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CommonModule.Announcements.Commands.CreateAnnouncement;
using Yonetim360Business.CQRS.CommonModule.Announcements.Commands.DeleteAnnouncement;
using Yonetim360Business.CQRS.CRM.CrmAnnouncements.Commands.AssignAnnouncementToRepresentatives;
using Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncementById;
using Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncementByRepresentativeId;
using Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncements;
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrmAnnouncementController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignToRepresentative(AssignAnnouncementToRepresentativeCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteAnnouncementCommand { Id = id });
            return Ok(result);
        }

        [HttpGet("Representative/{RepresentativeId}")]
        public async Task<List<ReadAnnouncementDto>> GetAnnouncementByRepresentativeId(Guid RepresentativeId)
        {
            var result = await Mediator.Send(new GetAnnouncementsByRepresentativeIdQuery { RepresentativeId = RepresentativeId });
            return result;
        }

        [HttpGet]
        public async Task<List<ReadAnnouncementDto>> GetAllAnnouncements(int pageSize=50,int pageNumber=1)
        {
            var result = await Mediator.Send(new GetAnnouncementsQuery { PageSize = pageSize, PageNumber = pageNumber });
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ReadAnnouncementDto> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetAnnouncementByIdQuery { Id = id });
            return result;
        }
    }
}
