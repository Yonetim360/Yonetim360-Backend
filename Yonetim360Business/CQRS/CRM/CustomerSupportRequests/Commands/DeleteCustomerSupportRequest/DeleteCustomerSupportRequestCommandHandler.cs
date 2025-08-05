using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.DeleteCustomerSupportRequest
{
    public class DeleteCustomerSupportRequestCommandHandler : ICommandHandler<DeleteCustomerSupportRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CustomerSupportRequest> _repository;
        public DeleteCustomerSupportRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
        }

        public async Task<bool> Handle(DeleteCustomerSupportRequestCommand request, CancellationToken cancellationToken)
        {
            var support = await _repository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new InvalidDataException("Not found Support Request");
            await _repository.DeleteAsync(support);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
