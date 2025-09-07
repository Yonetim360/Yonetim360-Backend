using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity;
using Yonetim360.Entity.İK;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.İK.İkAnnouncements.Commands.AssignAnnouncementToDeparments
{
    public class AssignAnnouncementToDepartmentsCommandHandler : ICommandHandler<AssignAnnouncementToDepartmentsCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<AnnouncementDepartment> _announcementDepartmentsRepository;
        public AssignAnnouncementToDepartmentsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepository = _unitOfWork.GetRepository<Announcement>();
            _departmentRepository = _unitOfWork.GetRepository<Department>();
            _announcementDepartmentsRepository = _unitOfWork.GetRepository<AnnouncementDepartment>();
        }

        public async Task<bool> Handle(AssignAnnouncementToDepartmentsCommand request, CancellationToken cancellationToken)
        {
            var announcement = await _announcementRepository.GetFirstOrDefaultAsync(x => x.Id == request.AnnouncementId) ??
                               throw new InvalidDataException("Not found announcement");

            var department = await _departmentRepository.GetAllAsync(x => request.DepartmentIds.Contains(x.Id)) ??
                 throw new InvalidDataException("Not found department");

            var existingAssignment = await _announcementDepartmentsRepository.GetAllAsync(x => x.AnnouncementId == request.AnnouncementId) ??
                    throw new InvalidDataException("Not found announcement departments");

            //duplicate kayıt olmasını engellemek için önce ilgili anonsun mevcuttaki department atamalarını siliyorum 
            foreach (var assignment in existingAssignment)
            {
                await _announcementDepartmentsRepository.DeleteAsync(assignment);
            }

            //her departman için ayrı ayrı kayıt oluşturuyorum
            foreach (var departments in request.DepartmentIds)
            {
                var announcementDepartment = new AnnouncementDepartment
                {
                    AnnouncementId = request.AnnouncementId,
                    DepartmentId = departments,
                    AssignedDate = DateTime.Now,
                    TenantId = announcement.TenantId
                };

                await _announcementDepartmentsRepository.CreateAsync(announcementDepartment);
                
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;


        }
    }
}
