using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomerById;
using Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.CreateOffer;
using Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.DeleteOffer;
using Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.UpdateOffer;
using Yonetim360Business.CQRS.CRM.OfferAndSales.Queries.GetOfferById;
using Yonetim360Business.CQRS.CRM.OfferAndSales.Queries.GetOffers;

namespace Yonetim360.API.Controllers.CRM.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateOfferCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteOfferCommand { Id = id };
            var result = await Mediator.Send(command);
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int PageSize=50,int PageNumber=1)
        {
            var result= await Mediator.Send( new GetOffersQuery { PageSize = PageSize, PageNumber = PageNumber });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetOfferByIdQuery { Id = id });
            return Ok(result);
        }
    }
}
