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

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.UpdateCustomerSupportRequest
{
    public class UpdateCustomerSupportRequestCommandHandler : ICommandHandler<UpdateCustomerSupportRequestCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CustomerSupportRequest> _repository;
        private readonly IRepository<User> _userRepository;
        public UpdateCustomerSupportRequestCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<bool> Handle(UpdateCustomerSupportRequestCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CustomerSupportRequestDto.UserId) ??
                throw new InvalidDataException("User not found");

            var updatedCustomerSupportRequest = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.CustomerSupportRequestDto.Id);

            _mapper.Map(request.CustomerSupportRequestDto, updatedCustomerSupportRequest);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
