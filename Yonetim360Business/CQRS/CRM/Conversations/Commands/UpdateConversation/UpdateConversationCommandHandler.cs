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
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateConversationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _conversationRepository = _unitOfWork.GetRepository<Conversation>();
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<bool> Handle(UpdateConversationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x=>x.Id == request.ConversationDto.UserId) ??
                throw new InvalidDataException("User not found");

            var newConversation = _mapper.Map<Conversation>(request.ConversationDto);
            await _conversationRepository.UpdateAsync(newConversation);
            return true;
        }
    }
}
