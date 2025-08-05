using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Yonetim360Business.CQRS.CRM.CrmTasks.Commands.CreateCrmTask;
using Yonetim360Business.CQRS.CRM.CrmTasks.Commands.DeleteCrmTask;
using Yonetim360Business.CQRS.CRM.CrmTasks.Commands.UpdateCrmTask;
using Yonetim360Business.CQRS.CRM.CrmTasks.Queries.GetCrmTaskById;
using Yonetim360Business.CQRS.CRM.CrmTasks.Queries.GetCrmTasks;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrmTaskController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCrmTaskCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCrmTaskCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delte(Guid id)
        {
            var result = await Mediator.Send(new DeleteCrmTaskCommand { Id = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize=50,int pageNumber=1)
        {
            var result = await Mediator.Send(new GetCrmTasksQuery { PageSize = pageSize, PageNumber = pageNumber });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetCrmTaskByIdQuery { Id = id });
            return Ok(result);
        }
    }
}
