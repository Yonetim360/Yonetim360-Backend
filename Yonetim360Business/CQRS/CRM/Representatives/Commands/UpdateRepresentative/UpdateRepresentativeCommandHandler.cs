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

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.UpdateRepresentative
{
    public class UpdateRepresentativeCommandHandler : ICommandHandler<UpdateRepresentativeCommand, bool>
    {
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateRepresentativeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<bool> Handle(UpdateRepresentativeCommand request, CancellationToken cancellationToken)
        {
           var UpdatedUser = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id==request.RepresentativeDto.UserId)??
                throw new Exception("User not found");

            var updatedRepresentative = await _representativeRepository.GetFirstOrDefaultAsync(x => x.Id == request.RepresentativeDto.Id) ??
                throw new Exception("Representative not found");

            _mapper.Map(request.RepresentativeDto,updatedRepresentative);

            await _unitOfWork.CommitAsync();
            return true;    

        }
    }
}
