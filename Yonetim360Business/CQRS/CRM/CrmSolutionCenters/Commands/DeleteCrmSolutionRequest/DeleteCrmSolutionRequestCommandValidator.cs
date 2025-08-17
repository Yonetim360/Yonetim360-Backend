using Azure.Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.DeleteSolutionRequest
{
    public class DeleteCrmSolutionRequestCommandValidator:AbstractValidator<DeleteCrmSolutionRequestCommand>
    {
        public DeleteCrmSolutionRequestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required")
                .NotNull().WithMessage("Id cannot be null");
        }
    }
}
