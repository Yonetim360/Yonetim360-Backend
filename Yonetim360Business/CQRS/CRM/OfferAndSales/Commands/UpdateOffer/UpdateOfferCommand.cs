using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.UpdateOffer
{
    public class UpdateOfferCommand:ICommand<bool>
    {
        public required OfferDto OfferDto { get; set; }
    }
}
