using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.Representatives.Commands.CreateRepresentative;
using Yonetim360Business.CQRS.CRM.Representatives.Commands.DeleteRepresentative;
using Yonetim360Business.CQRS.CRM.Representatives.Commands.UpdateRepresentative;
using Yonetim360Business.CQRS.CRM.Representatives.Queries.GetRepresentativeById;
using Yonetim360Business.CQRS.CRM.Representatives.Queries.GetRepresentatives;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentativeController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateRepresentativeCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);

        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRepresentativeCommand command)
        {

            var result = await Mediator.Send(command);
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteRepresentativeCommand { Id = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize=50,int pageNumber=1)
        {
            var result = await Mediator.Send(new GetRepresentativesQuery { PageSize = pageSize, PageNumber = pageNumber });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetRepresentativeByIdQuery { Id = id });
            return Ok(result);
        }

    }
}
