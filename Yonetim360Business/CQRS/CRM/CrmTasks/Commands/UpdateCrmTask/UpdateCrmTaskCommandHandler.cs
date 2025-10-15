using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Representative> _repRepository;
        public UpdateCrmTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<CrmTask>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            _repRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<bool> Handle(UpdateCrmTaskCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.CrmTaskDto.UpdatedBy)
        ?? throw new InvalidDataException("ApplicationUser not found");

            // 2️⃣ ✅ Representative'leri de yükle
            var crmTask = await _repository.GetFirstOrDefaultAsync(
                filter: x => x.Id == request.CrmTaskDto.Id,
                include: q => q.Include(t => t.Representative) // ✅ Include ekle
            ) ?? throw new InvalidDataException("Task not found");

            // 3️⃣ DTO'dan temel alanları güncelle
            _mapper.Map(request.CrmTaskDto, crmTask);

            // 4️⃣ Representative güncelleme
            if (request.CrmTaskDto.RepresentativeIds != null && request.CrmTaskDto.RepresentativeIds.Any())
            {
                // ✅ Mevcut representative'leri tracking ile çek
                var reps = await _repRepository.GetAllAsync(
                    filter: r => request.CrmTaskDto.RepresentativeIds.Contains(r.Id),
                    tracked: true // ✅ Tracking açık olsun
                );

                // ✅ Önce eskileri temizle
                crmTask.Representative.Clear();

                // ✅ Yenileri ekle (tracking olduğu için attach'e gerek yok)
                foreach (var rep in reps)
                {
                    crmTask.Representative.Add(rep);
                }
            }
            else
            {
                // ✅ Eğer liste boşsa, tüm representative'leri kaldır
                crmTask.Representative  .Clear();
            }

            // 5️⃣ Commit işlemi
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
