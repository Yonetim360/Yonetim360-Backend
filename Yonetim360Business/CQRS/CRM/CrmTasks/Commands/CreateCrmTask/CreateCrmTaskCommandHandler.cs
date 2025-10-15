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
        private readonly IRepository<Representative> _representativeRepository;
        public CreateCrmTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<CrmTask>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<CrmTaskDto> Handle(CreateCrmTaskCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CreatedBy)
        ?? throw new InvalidDataException("ApplicationUser not found");

            var newCrmTask = _mapper.Map<CrmTask>(request)
                ?? throw new InvalidDataException("Mapping failed");

            // ✅ DOĞRU - Mevcut entity'leri tek tek ekle
            if (request.RepresentativeIds != null && request.RepresentativeIds.Any())
            {
                var existingRepresentatives = await _representativeRepository
                    .GetAllAsync(
                    filter:x => request.RepresentativeIds.Contains(x.Id),
                    tracked:true
                    );

                // ✅ Her bir representative'i collection'a ekle
                foreach (var rep in existingRepresentatives)
                {
                    newCrmTask.Representative.Add(rep);
                }
            }

            await _repository.CreateAsync(newCrmTask);
            await _unitOfWork.CommitAsync();

            var crmTaskDto = _mapper.Map<CrmTaskDto>(newCrmTask)
                ?? throw new InvalidDataException("Mapping to DTO failed");

            return crmTaskDto;

        }
    }
}
