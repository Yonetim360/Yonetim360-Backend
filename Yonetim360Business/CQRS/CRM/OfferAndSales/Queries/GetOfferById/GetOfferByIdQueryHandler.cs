using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Queries.GetOfferById
{
    public class GetOfferByIdQueryHandler : IQueryHandler<GetOfferByIdQuery, OfferDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Offer> _offerRepository;
        public GetOfferByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _offerRepository = _unitOfWork.GetRepository<Offer>();
        }

        public async Task<OfferDto> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetFirstOrDefaultAsync(x=>x.Id==request.Id)??
                throw new Exception("Teklif bulunamadı.");
            return _mapper.Map<OfferDto>(offer);
        }
    }
}
