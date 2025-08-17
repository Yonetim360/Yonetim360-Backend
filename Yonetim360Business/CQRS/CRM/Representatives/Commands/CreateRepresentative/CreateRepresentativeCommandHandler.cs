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

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.CreateRepresentative
{
    public class CreateRepresentativeCommandHandler : ICommandHandler<CreateRepresentativeCommand, RepresentativeDto>
    {
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _userRepository;
        public CreateRepresentativeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<RepresentativeDto> Handle(CreateRepresentativeCommand request, CancellationToken cancellationToken)
        {
           var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id==request.UserId)??
                throw new InvalidDataException("ApplicationUser not found");

            var representative = _mapper.Map<Representative>(request);
            await _representativeRepository.CreateAsync(representative);
            await _unitOfWork.CommitAsync();

            var representativeDto = _mapper.Map<RepresentativeDto>(representative) ??
                throw new InvalidDataException("Representative could not be created");
            return representativeDto;
        }
    }
}
