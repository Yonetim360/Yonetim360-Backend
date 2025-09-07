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

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Commands.AssignAnnouncementToRepresentatives
{
    public class AssignAnnouncementToRepresentativeCommandHandler : ICommandHandler<AssignAnnouncementToRepresentativeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<AnnouncementRepresentative> _announcementRepresentativeRepository;
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public AssignAnnouncementToRepresentativeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepository = _unitOfWork.GetRepository<Announcement>();
            _announcementRepresentativeRepository = _unitOfWork.GetRepository<AnnouncementRepresentative>();
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(AssignAnnouncementToRepresentativeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.AssignedBy) ??
                 throw new InvalidDataException("The user who made the assignment could not be found.");

            var representatives = await _representativeRepository.GetAllAsync(x=>request.RepresentativeIds.Contains(x.Id))??
                throw new InvalidDataException("No representatives found to assign.");

            var announcement = await _announcementRepository.GetFirstOrDefaultAsync(x => x.Id == request.AnnouncementId) ??
                throw new InvalidDataException("Announcement not found.");

            // duplicate kayıt olmaması için önceki kayıtları siliyorum 
            var existingAssignments = await _announcementRepresentativeRepository.GetAllAsync(x=>x.AnnouncementId==request.AnnouncementId)??
                throw new InvalidDataException("No existing assignments found.");
            foreach (var assignment in existingAssignments) { 

             await _announcementRepresentativeRepository.DeleteAsync(assignment);

            }

           

            // yeni kayıtları her bir representative için ekliyorum 
            foreach (var representative in request.RepresentativeIds)
            {
                var Announcement = new AnnouncementRepresentative
                {
                    AnnouncementId = request.AnnouncementId,
                    RepresentativeId = representative,
                    AssignedBy = request.AssignedBy,
                    AssignedDate = request.AssignedDate,
                    TenantId = announcement.TenantId
                };
                await _announcementRepresentativeRepository.CreateAsync(Announcement);

            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
