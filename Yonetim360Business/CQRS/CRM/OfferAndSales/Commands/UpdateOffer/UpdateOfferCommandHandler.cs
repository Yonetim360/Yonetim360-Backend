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
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.UpdateOffer
{
    public class UpdateOfferCommandHandler : ICommandHandler<UpdateOfferCommand, bool>
    {
        private readonly IRepository<Offer> _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        public UpdateOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _offerRepository = _unitOfWork.GetRepository<Offer>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<bool> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.OfferDto.UserId) ??
                 throw new InvalidDataException("Not found an user");
            var updatedOffer= await _offerRepository.GetFirstOrDefaultAsync(x => x.Id == request.OfferDto.Id);

            _mapper.Map(request.OfferDto, updatedOffer);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
