using AutoMapper;
using Microsoft.Identity.Client;
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

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.UpdateSolutionRequest
{
    public class UpdateCrmSolutionRequestCommandHandler : ICommandHandler<UpdateCrmSolutionRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CrmSolutionRequest> _repository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public UpdateCrmSolutionRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository=_unitOfWork.GetRepository<CrmSolutionRequest>();
            _userRepository=unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(UpdateCrmSolutionRequestCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CrmSolutionRequestDto.UserId) ??
                 throw new InvalidDataException("Not found ApplicationUser");

            var solutionRequest = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.CrmSolutionRequestDto.Id) ??
                throw new InvalidDataException("Not found solution request");

            _mapper.Map(request.CrmSolutionRequestDto, solutionRequest);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
