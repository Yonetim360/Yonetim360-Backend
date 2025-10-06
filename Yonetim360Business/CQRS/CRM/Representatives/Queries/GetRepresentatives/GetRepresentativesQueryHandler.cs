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
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Representatives.Queries.GetRepresentatives
{
    public class GetRepresentativesQueryHandler : IQueryHandler<GetRepresentativesQuery, List<ReadRepresentativeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Representative> _repository;
        public GetRepresentativesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<List<ReadRepresentativeDto>> Handle(GetRepresentativesQuery request, CancellationToken cancellationToken)
        {
            var representatives = await _repository.GetAllAsync(null,false, request.PageSize,request.PageNumber) ??
                throw new InvalidDataException("Not found query");

           return _mapper.Map<List<ReadRepresentativeDto>>(representatives.ToList());
        }
    }
}
