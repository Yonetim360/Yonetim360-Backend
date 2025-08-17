using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.DeleteConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversationStatus;
using Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversationById;
using Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversations;

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteConversationCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id,UpdateConversationStatusCommand command)
        {
            command.ConversationId = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int PageSize=50,int PageNumber=1)
        {
           var result = await Mediator.Send( new GetConversationQuery { PageSize=PageSize, PageNumber = PageNumber });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetConversationByIdQuery { Id = id });
            return Ok(result);
        }
    }
}
