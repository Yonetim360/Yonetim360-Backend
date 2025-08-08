using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.CreateCustomerSupportRequest
{
    public class CreateCustomerSupportRequestCommandHandler : ICommandHandler<CreateCustomerSupportRequestCommand, CustomerSupportRequestDto>
    {
        private readonly IRepository<CustomerSupportRequest> _repository;
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        public CreateCustomerSupportRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
            _userRepository = _unitOfWork.GetRepository<User>();
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<CustomerSupportRequestDto> Handle(CreateCustomerSupportRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                throw new InvalidDataException("User not found");

            var suppoort = _mapper.Map<CustomerSupportRequest>(request) ??
                throw new InvalidDataException("Mapping failed");

            if (request.RepresentativeIds?.Any() == true)
            {
                var representatives = await _representativeRepository
                    .GetAllAsync(q => request.RepresentativeIds.Contains(q.Id));

                //Entity state'lerini unchanged yap
                foreach (var representative in representatives)
                {
                    _unitOfWork.SetEntityState(representative, EntityState.Unchanged);
                }

                // Conversation'a ekle
                suppoort.Representatives = representatives.ToList();
            }
            await _repository.CreateAsync(suppoort);
            await _unitOfWork.CommitAsync();

            var customerSupportRequestDto = _mapper.Map<CustomerSupportRequestDto>(suppoort) ??
                throw new InvalidDataException("Mapping failed");
            return customerSupportRequestDto;
        }
    }
}
