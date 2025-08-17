using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Queires.GetCrmSolutionRequests
{
    public class GetCrmSolutionRequestsQueryHandler : IQueryHandler<GetCrmSolutionRequestsQuery, List<CrmSolutionRequestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CrmSolutionRequest> _repository;
        public GetCrmSolutionRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository=_unitOfWork.GetRepository<CrmSolutionRequest>();
        }

        public async Task<List<CrmSolutionRequestDto>> Handle(GetCrmSolutionRequestsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsync(null, false, request.PageSize, request.PageNumber) ??
                 throw new InvalidDataException("Not found solutions");

            return _mapper.Map<List<CrmSolutionRequestDto>>(query);
        }
    }
}
