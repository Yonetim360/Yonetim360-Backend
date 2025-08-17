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

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.DeleteRepresentative
{
    public class DeleteRepresentativeCommandHandler : ICommandHandler<DeleteRepresentativeCommand, bool>
    {
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRepresentativeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository=_unitOfWork.GetRepository<ApplicationUser>();
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<bool> Handle(DeleteRepresentativeCommand request, CancellationToken cancellationToken)
        {


            var deletedRepresentative = await _representativeRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Representative not found");

            await _representativeRepository.DeleteAsync(deletedRepresentative);
            await _unitOfWork.CommitAsync();
            return true;





        }
    }
}
