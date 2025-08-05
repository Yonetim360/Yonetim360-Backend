using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.UpdateCrmTask
{
    public class UpdateCrmTaskCommand:ICommand<bool>
    {
        public required CrmTaskDto CrmTaskDto { get; set; }
    }
}
