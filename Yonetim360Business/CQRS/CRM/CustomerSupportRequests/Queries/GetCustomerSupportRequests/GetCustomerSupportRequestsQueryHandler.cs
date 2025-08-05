using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequests
{
    public class GetCustomerSupportRequestsQueryHandler : IQueryHandler<GetCustomerSupportRequestsQuery, List<CustomerSupportRequestDto>>
    {
        private readonly IRepository<CustomerSupportRequest> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCustomerSupportRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
            _mapper = mapper;
        }

        public async Task<List<CustomerSupportRequestDto>> Handle(GetCustomerSupportRequestsQuery request, CancellationToken cancellationToken)
        {
            var supports = await _repository.GetAllAsync(
                filter: null,
                tracked: false,
                pageSize: request.PageSize,
                pageNumber: request.PageNumber,
                include:q=>q.Include(r=>r.Representatives)) ?? throw new InvalidDataException("Not found any customer support request records");

            return _mapper.Map<List<CustomerSupportRequestDto>>(supports);
        }
    }
}
