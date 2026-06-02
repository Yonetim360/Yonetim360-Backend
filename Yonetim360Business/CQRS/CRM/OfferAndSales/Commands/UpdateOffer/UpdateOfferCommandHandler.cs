using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.UpdateOffer
{
    public class UpdateOfferCommandHandler : ICommandHandler<UpdateOfferCommand, bool>
    {
        private readonly IRepository<Offer> _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationUser> _userRepository;
        public UpdateOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _offerRepository = _unitOfWork.GetRepository<Offer>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.OfferDto.UpdatedBy)
                ?? throw new InvalidDataException("Not found an ApplicationUser");

            var existingOffer = await _offerRepository.GetFirstOrDefaultAsync(x => x.Id == request.OfferDto.Id)
                ?? throw new InvalidDataException("Offer not found");

            // Map gelen DTO'daki güncel verileri mevcut entity'e uygula
            _mapper.Map(request.OfferDto, existingOffer);

            // FinalAmount'ı yeniden hesapla
            switch (existingOffer.DiscountType)
            {
                case DiscountType.Percentage:
                    existingOffer.FinalAmount = existingOffer.Amount - (existingOffer.Amount * existingOffer.DiscountValue / 100);
                    break;
                case DiscountType.FixedAmount:
                    existingOffer.FinalAmount = existingOffer.Amount - existingOffer.DiscountValue;
                    break;
                case DiscountType.None:
                default:
                    existingOffer.FinalAmount = existingOffer.Amount;
                    break;
            }

            await _unitOfWork.CommitAsync();
            return true;
        }

    }
}
