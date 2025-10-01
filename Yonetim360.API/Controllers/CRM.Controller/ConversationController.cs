using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360.Entity.CRM;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.DeleteConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversationStatus;
using Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversationById;
using Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, UpdateConversationStatusCommand request)
        {


            request.ConversationId = id;
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? CustomerId, ConversationStatus? conversationStatus, int PageSize=50,int PageNumber=1)
        {
           var result = await Mediator.Send( new GetConversationQuery {CustomerId=CustomerId,ConversationStatus=conversationStatus,PageSize=PageSize, PageNumber = PageNumber });
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
