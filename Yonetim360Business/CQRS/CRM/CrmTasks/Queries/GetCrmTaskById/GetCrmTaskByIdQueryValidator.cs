using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Queries.GetCrmTaskById
{
    public class GetCrmTaskByIdQueryValidator:AbstractValidator<GetCrmTaskByIdQuery>
    {
        public GetCrmTaskByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty").NotNull().WithMessage("Id cannot be null");    
        }
    }
}
