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
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.UpdateCustomerSupportRequest
{
    public class UpdateCustomerSupportRequestCommandHandler : ICommandHandler<UpdateCustomerSupportRequestCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CustomerSupportRequest> _repository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public UpdateCustomerSupportRequestCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(UpdateCustomerSupportRequestCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CustomerSupportRequestDto.UpdatedBy)
                ?? throw new InvalidDataException("ApplicationUser not found");

            var updatedCustomerSupportRequest = await _repository.GetFirstOrDefaultAsync(
                x => x.Id == request.CustomerSupportRequestDto.Id,
                include: q => q.Include(c => c.Representatives)
            ) ?? throw new InvalidDataException("CustomerSupportRequest not found");

            // Temel alanları güncelle
            updatedCustomerSupportRequest.Subject = request.CustomerSupportRequestDto.Subject;
            updatedCustomerSupportRequest.Explanation = request.CustomerSupportRequestDto.Explanation;
            updatedCustomerSupportRequest.Priority = request.CustomerSupportRequestDto.Priority;
            updatedCustomerSupportRequest.SupportRequestStatus = request.CustomerSupportRequestDto.SupportRequestStatus;
            updatedCustomerSupportRequest.UpdatedBy = request.CustomerSupportRequestDto.UpdatedBy;
            // Representatives güncelle
            updatedCustomerSupportRequest.Representatives ??= new List<Representative>();

            // Önce mevcut ilişkileri temizle
            updatedCustomerSupportRequest.Representatives.Clear();

            // Yeni Id’leri ekle
            if (request.CustomerSupportRequestDto.RepresentativeIds != null)
            {
                var repRepo = _unitOfWork.GetRepository<Representative>();
                foreach (var repId in request.CustomerSupportRequestDto.RepresentativeIds)
                {
                    var rep = await repRepo.GetFirstOrDefaultAsync(r => r.Id == repId);
                    if (rep != null)
                        updatedCustomerSupportRequest.Representatives.Add(rep);
                }
            }

            await _unitOfWork.CommitAsync();
            return true;
        }



    }
}
