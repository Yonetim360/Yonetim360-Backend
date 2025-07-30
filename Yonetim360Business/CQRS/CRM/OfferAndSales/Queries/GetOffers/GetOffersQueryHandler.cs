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

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Queries.GetOffers
{
    public class GetOffersQueryHandler : IQueryHandler<GetOffersQuery, List<OfferDto>>
    {
        private readonly IRepository<Offer> _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOffersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _offerRepository = _unitOfWork.GetRepository<Offer>();
            _mapper = mapper;
        }

        public async Task<List<OfferDto>> Handle(GetOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetAllAsync(null,false,request.PageSize,request.PageNumber)??
                throw new InvalidDataException("Teklifler bulunamadı.");
            return _mapper.Map<List<OfferDto>>(offers);
        }
    }
}
