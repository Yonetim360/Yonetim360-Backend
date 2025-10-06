using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Queires.GetCrmSolutionRequestById
{
    public class GetCrmSolutionRequestByIdQueryHandler : IQueryHandler<GetCrmSolutionRequestByIdQuery, ReadCrmSolutionRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CrmSolutionRequestDto> _repository;
        public GetCrmSolutionRequestByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository=_unitOfWork.GetRepository<CrmSolutionRequestDto>();
        }

        public async Task<ReadCrmSolutionRequestDto> Handle(GetCrmSolutionRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.Id) ??
                throw new InvalidDataException("Not found query");
            return _mapper.Map<ReadCrmSolutionRequestDto>(query);
        }
    }
}
