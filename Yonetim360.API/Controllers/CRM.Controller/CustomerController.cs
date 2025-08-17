using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer;
using Yonetim360Business.CQRS.CRM.Customers.Commands.DeleteCustomer;
using Yonetim360Business.CQRS.CRM.Customers.Commands.UpdateCustomer;
using Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomerById;
using Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomers;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteCustomerCommand { Id=id});
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageSize = 50, int pageNumber = 1)
        {
            var result = await Mediator.Send(new GetCustomersQuery { PageSize = pageSize, PageNumber = pageNumber });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetCustomerByIdQuery { Id = id });
            return Ok(result);
        }
    }
}
