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

namespace Yonetim360Business.CQRS.CommonModule.Announcements.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementCommandHandler : ICommandHandler<DeleteAnnouncementCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Announcement> _announcementRepository;
        public DeleteAnnouncementCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepository = _unitOfWork.GetRepository<Announcement>();
        }

        public async Task<bool> Handle(DeleteAnnouncementCommand request, CancellationToken cancellationToken)
        {
           var announcement = await _announcementRepository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new InvalidDataException("Duyuru bulunamadı");

            await _announcementRepository.DeleteAsync(announcement);
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
