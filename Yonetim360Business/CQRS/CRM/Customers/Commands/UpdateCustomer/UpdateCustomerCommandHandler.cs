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

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _customerRepository;
        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerRepository = _unitOfWork.GetRepository<Customer>();
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var UpdatedCustomer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Id == request.CustomerDto.Id);
            UpdatedCustomer= _mapper.Map<Customer>(request.CustomerDto);
            await _customerRepository.UpdateAsync(UpdatedCustomer);
            await _unitOfWork.CommitAsync();
            return true;

        }
    }
}
