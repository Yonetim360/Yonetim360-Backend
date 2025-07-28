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

namespace Yonetim360Business.CQRS.CRM.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, List<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _customerRepository;
        public GetCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerRepository = _unitOfWork.GetRepository<Customer>();
        }

        public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customerList= await _customerRepository.GetAllAsync(null,false,request.PageSize,request.PageNumber)
                ?? throw new ArgumentException("No customer found. Please make a valid request");
            var result = _mapper.Map<List<CustomerDto>>(customerList.ToList());
            return result;
        }
    }
}
