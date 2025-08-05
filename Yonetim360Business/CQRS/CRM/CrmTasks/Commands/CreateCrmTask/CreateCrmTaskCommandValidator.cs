using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.CreateCrmTask
{
    public class CreateCrmTaskCommandValidator:AbstractValidator<CreateCrmTaskCommand>
    {
        public CreateCrmTaskCommandValidator()
        {
            RuleFor(x=>x.UserId)
                .NotEmpty().WithMessage("User ID cannot be empty.");
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Task title cannot be empty.")
                .MaximumLength(100).WithMessage("Task title cannot exceed 100 characters.");
            RuleFor(x => x.TaskCategory).IsInEnum()
                .WithMessage("Task category must be a valid enum value.").NotEmpty().WithMessage("Category cannot empty");
            RuleFor(x=>x.RepresentativeId).NotEmpty()
                .WithMessage("Representative ID cannot be empty.");
            RuleFor(x => x.Description).Length(0, 500)
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
