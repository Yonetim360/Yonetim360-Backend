using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.DeleteSolutionRequest
{
    public class DeleteCrmSolutionRequestCommandHandler : ICommandHandler<DeleteCrmSolutionRequestCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CrmSolutionRequest> _repository;
        public DeleteCrmSolutionRequestCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CrmSolutionRequest>();
        }

        public async Task<bool> Handle(DeleteCrmSolutionRequestCommand request, CancellationToken cancellationToken)
        {
            var crmSolution = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.Id) ??
                 throw new InvalidDataException("Not found crmSolution");

            await _repository.DeleteAsync(crmSolution);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
