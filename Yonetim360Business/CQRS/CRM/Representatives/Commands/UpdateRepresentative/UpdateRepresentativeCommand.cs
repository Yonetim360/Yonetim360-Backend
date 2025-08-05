using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.UpdateRepresentative
{
    public class UpdateRepresentativeCommand:ICommand<bool>
    {
        public required RepresentativeDto RepresentativeDto { get; set; }
    }
}
