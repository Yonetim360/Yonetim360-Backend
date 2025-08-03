using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.Representatives.CreateRepresentative;

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
    }
}
