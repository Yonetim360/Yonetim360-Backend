using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Yonetim360.Entity;
using Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.CreateSolutionRequest;
using Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.DeleteSolutionRequest;
using Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.UpdateSolutionRequest;
using Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Queires.GetCrmSolutionRequestById;
using Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Queires.GetCrmSolutionRequests;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrmSolutionRequestController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCrmSolutionRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteCrmSolutionRequestCommand { Id=id});
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCrmSolutionRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 50, int pageNumber = 1)
        {
            var result = await Mediator.Send(new GetCrmSolutionRequestsQuery { PageSize = pageSize, PageNumber = pageNumber });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetCrmSolutionRequestByIdQuery { Id = id });   
            return Ok(result);
        }
    }
}
