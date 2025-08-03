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

namespace Yonetim360Business.CQRS.CRM.Representatives.CreateRepresentative
{
    public class CreateRepresentativeCommandHandler : ICommandHandler<CreateRepresentativeCommand, bool>
    {
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        public CreateRepresentativeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<bool> Handle(CreateRepresentativeCommand request, CancellationToken cancellationToken)
        {
           var user = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id==request.UserId)??
                throw new InvalidDataException("User not found");

            var representative = _mapper.Map<Representative>(request);
            await _representativeRepository.CreateAsync(representative);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
