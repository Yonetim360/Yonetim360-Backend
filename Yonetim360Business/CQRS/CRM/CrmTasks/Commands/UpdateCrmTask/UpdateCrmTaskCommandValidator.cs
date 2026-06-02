using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.UpdateCrmTask
{
    public class UpdateCrmTaskCommandValidator:AbstractValidator<UpdateCrmTaskCommand>
    {
        public UpdateCrmTaskCommandValidator()
        {
            RuleFor(x => x.CrmTaskDto.Id)
                .NotEmpty().WithMessage("Task ID cannot be empty.")
                .NotNull().WithMessage("Task ID cannot be null.");
            RuleFor(x => x.CrmTaskDto.UpdatedBy)
                .NotEmpty().WithMessage("ApplicationUser ID cannot be empty.");
            RuleFor(x => x.CrmTaskDto.Title).NotEmpty()
                .WithMessage("Task title cannot be empty.")
                .MaximumLength(100).WithMessage("Task title cannot exceed 100 characters.");
            RuleFor(x => x.CrmTaskDto.TaskCategory).IsInEnum()
                .WithMessage("Task category must be a valid enum value.");
            RuleFor(x => x.CrmTaskDto.RepresentativeIds).NotEmpty()
                .WithMessage("Representative ID cannot be empty.");
            RuleFor(x => x.CrmTaskDto.Description).Length(0, 500)
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
