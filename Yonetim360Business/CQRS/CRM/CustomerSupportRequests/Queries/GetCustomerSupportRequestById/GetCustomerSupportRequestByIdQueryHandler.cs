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

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequestById
{
    public class GetCustomerSupportRequestByIdQueryHandler : IQueryHandler<GetCustomerSupportRequestByIdQuery, CustomerSupportRequestDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CustomerSupportRequest> _repository;
        public GetCustomerSupportRequestByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
        }

        public async Task<CustomerSupportRequestDto> Handle(GetCustomerSupportRequestByIdQuery request, CancellationToken cancellationToken)
        {
           var query = await _repository.GetFirstOrDefaultAsync(
               filter:x=>x.Id==request.Id,
               tracked:false,
               include:x=>x.Include(y=>y.Representatives)
               )?? throw new InvalidDataException("Not found a customer support request record");

          return _mapper.Map<CustomerSupportRequestDto>(query);
        }
    }
}
