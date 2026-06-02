using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.Users.AssignRolesToUser;
using Yonetim360Business.CQRS.Users.CreateUser;
using Yonetim360Business.CQRS.Users.DeleteUser;
using Yonetim360Business.CQRS.Users.Queries.GetTenantUserById;
using Yonetim360Business.CQRS.Users.Queries.GetTenantUsers;
using Yonetim360Business.CQRS.Users.UpdateUser;

namespace Yonetim360.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] bool onlyUnlinkedRepresentatives = false,
            [FromQuery] int pageSize = 100,
            [FromQuery] int pageNumber = 1)
        {
            var result = await Mediator.Send(new GetTenantUsersQuery
            {
                OnlyUnlinkedRepresentatives = onlyUnlinkedRepresentatives,
                PageSize = pageSize,
                PageNumber = pageNumber
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetTenantUserByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteUserCommand { Id = id });
            return Ok(result);
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> AssignRole(AssignRolesToUserCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
