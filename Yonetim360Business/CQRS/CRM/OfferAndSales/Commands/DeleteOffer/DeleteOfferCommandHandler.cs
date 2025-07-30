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

namespace Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.DeleteOffer
{
    public class DeleteOfferCommandHandler : ICommandHandler<DeleteOfferCommand, bool>
    {
        private readonly IRepository<Offer> _offerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        public DeleteOfferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _offerRepository=_unitOfWork.GetRepository<Offer>();
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<bool> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                 throw new InvalidDataException("User is not invalid");

            var deletedOffer = await _offerRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id) ??
                throw new InvalidDataException("Offer is not invalid");

            await _offerRepository.DeleteAsync(deletedOffer);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;

        }
    }
}
