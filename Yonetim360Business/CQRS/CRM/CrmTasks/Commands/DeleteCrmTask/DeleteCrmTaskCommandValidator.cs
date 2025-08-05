using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.DeleteCrmTask
{
    public class DeleteCrmTaskCommandValidator:AbstractValidator<DeleteCrmTaskCommand>
    {
        public DeleteCrmTaskCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x=>x.Id).NotNull().WithMessage("Id cannot null");
        }
    }
}
