using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Concrete;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CustomerDto>
    {
       private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerRepository = _unitOfWork.GetRepository<Customer>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id==request.UserId)??
                throw new InvalidDataException("ApplicationUser not found");
            
            var newCustomer =  _mapper.Map<Customer>(request) ?? throw new InvalidDataException("Mapping failed");
            await _customerRepository.CreateAsync(newCustomer);
           await _unitOfWork.CommitAsync();
            var customerDtoo = _mapper.Map<CustomerDto>(newCustomer);
            return customerDtoo;
        }
    }
}
