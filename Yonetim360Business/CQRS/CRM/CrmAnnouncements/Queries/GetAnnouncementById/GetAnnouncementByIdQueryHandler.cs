using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncementById
{
    public class GetAnnouncementByIdQueryHandler : IQueryHandler<GetAnnouncementByIdQuery, AnnouncementDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Announcement> _announcementRepository;
        public GetAnnouncementByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepository = _unitOfWork.GetRepository<Announcement>();
        }

        public async Task<AnnouncementDto> Handle(GetAnnouncementByIdQuery request, CancellationToken cancellationToken)
        {
           var query = await _announcementRepository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new InvalidDataException("Announcement not found");

           return _mapper.Map<AnnouncementDto>(query);
        }
    }
}
