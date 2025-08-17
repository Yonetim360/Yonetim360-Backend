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
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.CreateOffer
{
    public class CreateOfferCommandHandler : ICommandHandler<CreateOfferCommand, OfferDto>
    {
        private readonly IRepository<Offer> _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _userRepository;
        public CreateOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _offerRepository = _unitOfWork.GetRepository<Offer>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<OfferDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                throw new InvalidDataException("Not found an ApplicationUser");
            var offer = _mapper.Map<Offer>(request);

            switch (request.DiscountType)
            {
                case DiscountType.Percentage:
                    offer.FinalAmount = request.Amount - (request.Amount * request.DiscountValue / 100);
                    break;
                case DiscountType.FixedAmount:
                    offer.FinalAmount = request.Amount - request.DiscountValue;
                    break;
                case DiscountType.None:
                default:
                    offer.FinalAmount = request.Amount;
                    break;
            }

            await _offerRepository.CreateAsync(offer);
            await _unitOfWork.CommitAsync();
            var offerDto = _mapper.Map<OfferDto>(offer) ?? throw new InvalidDataException("Offer could not be created");
            return offerDto;
        }
    }
}
