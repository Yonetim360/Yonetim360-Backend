using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CommonModule.Announcements.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommandHandler : ICommandHandler<CreateAnnouncementCommand, Guid>
    {
        private readonly IRepository<Announcement> _announcementRepositoory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _userRepository;
        public CreateAnnouncementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepositoory = _unitOfWork.GetRepository<Announcement>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<Guid> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                throw new InvalidDataException("Not invalid user ");

            var announcement = _mapper.Map<Announcement>(request);
            await _announcementRepositoory.CreateAsync(announcement);
            await _unitOfWork.CommitAsync();
            return announcement.Id;
        }
    }
}
