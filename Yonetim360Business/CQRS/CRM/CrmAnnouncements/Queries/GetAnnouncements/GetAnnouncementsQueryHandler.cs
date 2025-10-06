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
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncements
{
    public class GetAnnouncementsQueryHandler : IQueryHandler<GetAnnouncementsQuery, List<ReadAnnouncementDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Announcement> _announcementRepository;
        public GetAnnouncementsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _announcementRepository = _unitOfWork.GetRepository<Announcement>();
        }

        public async Task<List<ReadAnnouncementDto>> Handle(GetAnnouncementsQuery request, CancellationToken cancellationToken)
        {
           var query = await _announcementRepository.GetAllAsync(
               include:null,
               filter: null,
               pageSize: request.PageSize,
               pageNumber: request.PageNumber,
               tracked: false
           );

            return _mapper.Map<List<ReadAnnouncementDto>>(query);
        }
    }
}
