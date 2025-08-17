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

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.UpdateCrmTask
{
    public class UpdateCrmTaskCommandHandler : ICommandHandler<UpdateCrmTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CrmTask> _repository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public UpdateCrmTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<CrmTask>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(UpdateCrmTaskCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CrmTaskDto.UserId) ??
              throw new InvalidDataException("ApplicationUser not found");

            var crmTask = await _repository.GetFirstOrDefaultAsync(x => x.Id == request.CrmTaskDto.Id) ??
                 throw new InvalidDataException("Not found Task");
            
            _mapper.Map(request.CrmTaskDto, crmTask);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
