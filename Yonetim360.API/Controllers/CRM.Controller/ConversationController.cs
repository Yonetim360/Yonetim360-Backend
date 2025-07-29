using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.DeleteConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversation;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateConversationCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateConversationCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteConversationCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
