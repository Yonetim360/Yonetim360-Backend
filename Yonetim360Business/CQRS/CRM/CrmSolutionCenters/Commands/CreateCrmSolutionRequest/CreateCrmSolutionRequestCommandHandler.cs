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
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.CreateSolutionRequest
{
    public class CreateCrmSolutionRequestCommandHandler : ICommandHandler<CreateCrmSolutionRequestCommand,CrmSolutionRequestDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<CrmSolutionRequest> _crmSolutionRequestRepository;
        public CreateCrmSolutionRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository= _unitOfWork.GetRepository<ApplicationUser>();
            _crmSolutionRequestRepository=_unitOfWork.GetRepository<CrmSolutionRequest>();
        }

        public async Task<CrmSolutionRequestDto> Handle(CreateCrmSolutionRequestCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CreatedBy) ??
                 throw new InvalidDataException("Not found ApplicationUser");

          var solutionRequest= _mapper.Map<CrmSolutionRequest>(request);

            await _crmSolutionRequestRepository.CreateAsync(solutionRequest);
            await _unitOfWork.CommitAsync();

            var solut = _mapper.Map<CrmSolutionRequestDto>(solutionRequest);
            return solut;
        }
    }
}
