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

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversation
{
    public class UpdateConversationCommandHandler : ICommandHandler<UpdateConversationCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateConversationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _conversationRepository = _unitOfWork.GetRepository<Conversation>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<bool> Handle(UpdateConversationCommand request, CancellationToken cancellationToken)
        {
            var ApplicationUser = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id == request.ConversationDto.UserId) ??
                throw new InvalidDataException("ApplicationUser not found");

            var updatedConversation = await _conversationRepository.GetFirstOrDefaultAsync(x => x.Id == request.ConversationDto.Id);

           _mapper.Map(request.ConversationDto,updatedConversation);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
