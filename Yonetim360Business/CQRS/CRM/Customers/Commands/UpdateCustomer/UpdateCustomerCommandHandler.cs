using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _customerRepository = _unitOfWork.GetRepository<Customer>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CustomerDto.UpdatedBy) ??
                throw new InvalidDataException("ApplicationUser not found");
            var UpdatedCustomer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Id == request.CustomerDto.Id);
             _mapper.Map(request.CustomerDto, UpdatedCustomer);
            await _unitOfWork.CommitAsync();
            return true;

        }
    }
}
