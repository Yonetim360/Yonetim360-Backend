using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.CreateCrmTask
{
    public class CreateCrmTaskCommand:ICommand<CrmTaskDto>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public DateTime Time { get; set; }
        public Guid RepresentativeId { get; set; }
        public string Description { get; set; }
    }
}
