using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.CreateCrmTask
{
    public class CreateCrmTaskCommandHandler : ICommandHandler<CreateCrmTaskCommand, CrmTaskDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CrmTask> _repository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public CreateCrmTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<CrmTask>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<CrmTaskDto> Handle(CreateCrmTaskCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                throw new InvalidDataException("ApplicationUser not found");

            var newCrmTask = _mapper.Map<CrmTask>(request) ?? throw new InvalidDataException("Mapping failed");
            await _repository.CreateAsync(newCrmTask);
            await _unitOfWork.CommitAsync();

            var crmTaskDto = _mapper.Map<CrmTaskDto>(newCrmTask) ?? throw new InvalidDataException("Mapping to DTO failed");
            return crmTaskDto;

        }
    }
}
