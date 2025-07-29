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

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandHandler : ICommandHandler<CreateConversationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public CreateConversationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _conversationRepository = _unitOfWork.GetRepository<Conversation>();
            _userRepository = _unitOfWork.GetRepository<User>();
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                throw new InvalidDataException("User not found");

            var conversation = _mapper.Map<Conversation>(request) ?? throw new InvalidDataException("Mapping failed");
            await _conversationRepository.CreateAsync(conversation);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
