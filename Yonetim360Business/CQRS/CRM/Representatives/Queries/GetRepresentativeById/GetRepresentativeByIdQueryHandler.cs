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

namespace Yonetim360Business.CQRS.CRM.Representatives.Queries.GetRepresentativeById
{
    public class GetRepresentativeByIdQueryHandler : IQueryHandler<GetRepresentativeByIdQuery, ReadRepresentativeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Representative> _repository;
        private readonly IMapper _mapper;
        public GetRepresentativeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<ReadRepresentativeDto> Handle(GetRepresentativeByIdQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.Id) ??
                throw new InvalidDataException("Not found a representative");
           return _mapper.Map<ReadRepresentativeDto>(query);
        }
    }
}
