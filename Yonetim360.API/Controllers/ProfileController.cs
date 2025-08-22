using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.Profiles.Queries.GetCurrentProfile;

namespace Yonetim360.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {

        [Authorize]

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var result = await Mediator.Send(new GetCurrentProfileQuery());
            return Ok(result);
        }
    }
}
