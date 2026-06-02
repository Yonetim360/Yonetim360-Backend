using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Commands.UpdateCrmTask
{
    public class UpdateCrmTaskCommandHandler : ICommandHandler<UpdateCrmTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<CrmTask> _repository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<Representative> _repRepository;
        private readonly ICrmTaskAssignmentEmailHandler _assignmentEmailHandler;
        public UpdateCrmTaskCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICrmTaskAssignmentEmailHandler assignmentEmailHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _assignmentEmailHandler = assignmentEmailHandler;
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

            var existingRepresentativeIds = crmTask.Representative.Select(x => x.Id).ToHashSet();
            IEnumerable<Representative> newlyAssignedRepresentatives = Enumerable.Empty<Representative>();

            // 3️⃣ DTO'dan temel alanları güncelle
            _mapper.Map(request.CrmTaskDto, crmTask);

            // 4️⃣ Representative güncelleme
            var requestedRepresentativeIds = request.CrmTaskDto.RepresentativeIds?
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToList() ?? new List<Guid>();

            if (requestedRepresentativeIds.Any())
            {
                // ✅ Mevcut representative'leri tracking ile çek
                var reps = await _repRepository.GetAllAsync(
                    filter: r => requestedRepresentativeIds.Contains(r.Id),
                    tracked: true // ✅ Tracking açık olsun
                );
                var repList = reps.ToList();
                var foundRepresentativeIds = repList.Select(x => x.Id).ToHashSet();
                var missingRepresentativeIds = requestedRepresentativeIds
                    .Where(x => !foundRepresentativeIds.Contains(x))
                    .ToList();

                if (missingRepresentativeIds.Any())
                {
                    throw new InvalidDataException(
                        $"Representative not found or not accessible for this tenant. Ids: {string.Join(", ", missingRepresentativeIds)}");
                }

                // ✅ Önce eskileri temizle
                crmTask.Representative.Clear();

                // ✅ Yenileri ekle (tracking olduğu için attach'e gerek yok)
                foreach (var rep in repList)
                {
                    crmTask.Representative.Add(rep);
                }

                newlyAssignedRepresentatives = repList
                    .Where(x => !existingRepresentativeIds.Contains(x.Id))
                    .ToList();
            }
            else
            {
                // ✅ Eğer liste boşsa, tüm representative'leri kaldır
                crmTask.Representative  .Clear();
            }

            // 5️⃣ Commit işlemi
            await _unitOfWork.CommitAsync();
            await _assignmentEmailHandler.SendAssignmentEmailsAsync(crmTask, newlyAssignedRepresentatives);
            return true;
        }
    }
}
