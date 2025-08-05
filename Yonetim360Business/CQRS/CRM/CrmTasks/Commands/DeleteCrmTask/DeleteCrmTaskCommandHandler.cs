using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.DeleteCrmTask
{
    public class DeleteCrmTaskCommandHandler : ICommandHandler<DeleteCrmTaskCommand, bool>
    {
        private readonly IRepository<CrmTask> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCrmTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CrmTask>();
        }

        public async Task<bool> Handle(DeleteCrmTaskCommand request, CancellationToken cancellationToken)
        {
            var deletedTask = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.Id) 
                ?? throw new InvalidDataException("Not found task");
            await _repository.DeleteAsync(deletedTask);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
