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
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncementByRepresentativeId
{
    public class GetAnnouncementsByRepresentativeIdQueryHandler : IQueryHandler<GetAnnouncementsByRepresentativeIdQuery, List<ReadAnnouncementDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Announcement> _announcementRepository;
        private readonly IRepository<AnnouncementRepresentative> _announcementRepresentativeRepository;
        private readonly IRepository<Representative> _representativeRepository;
        public GetAnnouncementsByRepresentativeIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepository = _unitOfWork.GetRepository<Announcement>();
            _announcementRepresentativeRepository = _unitOfWork.GetRepository<AnnouncementRepresentative>();
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<List<ReadAnnouncementDto>> Handle(GetAnnouncementsByRepresentativeIdQuery request, CancellationToken cancellationToken)
        {
           var representative = await _representativeRepository.GetFirstOrDefaultAsync(r => r.Id == request.RepresentativeId)??
                throw new InvalidDataException("Not found a record for selected representative");

            var announcementrepresentatives = await _announcementRepresentativeRepository.GetAllAsync(
               filter: ar => ar.RepresentativeId == representative.Id,
               include: ar => ar.Include(a => a.Announcement),
                tracked: false
               ) ??
                throw new InvalidDataException("Not found a announcement record for a selected representativeId");

            var announcements = announcementrepresentatives
         .Select(ar => ar.Announcement) // Navigation property üzerinden erişim
         .ToList();

            return _mapper.Map<List<ReadAnnouncementDto>>(announcements);
        }
    }
}
