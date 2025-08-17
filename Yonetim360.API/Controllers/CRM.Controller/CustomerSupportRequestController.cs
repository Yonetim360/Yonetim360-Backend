using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.CreateCustomerSupportRequest;
using Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.DeleteCustomerSupportRequest;
using Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.UpdateCustomerSupportRequest;
using Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequestById;
using Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequests;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSupportRequestController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerSupportRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerSupportRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCustomerSupportRequestCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 50,int pageNumber=1) {
            
            var result = await Mediator.Send( new GetCustomerSupportRequestsQuery { PageSize = pageSize, PageNumber = pageNumber });
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetCustomerSupportRequestByIdQuery { Id = id });
            return Ok(result);

        }


    }
}
