using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.DeleteOffer
{
    public class DeleteOfferCommand:ICommand<bool>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
