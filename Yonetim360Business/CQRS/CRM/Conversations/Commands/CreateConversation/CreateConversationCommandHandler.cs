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
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommandHandler : ICommandHandler<CreateConversationCommand, ConversationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Representative> _representativeRepository;
        public CreateConversationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _conversationRepository = _unitOfWork.GetRepository<Conversation>();
            _userRepository = _unitOfWork.GetRepository<User>();
            _mapper = mapper;
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId) ??
                throw new InvalidDataException("User not found");

            var conversation = _mapper.Map<Conversation>(request) ??
                throw new InvalidDataException("Mapping failed");

            // UnitOfWork State Management
            if (request.RepresentativeIds?.Any() == true)
            {
                var representatives = await _representativeRepository
                    .GetAllAsync(x => request.RepresentativeIds.Contains(x.Id));

                //Entity state'lerini unchanged yap
                foreach (var representative in representatives)
                {
                    _unitOfWork.SetEntityState(representative, EntityState.Unchanged);
                }

                // Conversation'a ekle
                conversation.Representatives = representatives.ToList();
            }

            await _conversationRepository.CreateAsync(conversation);
            await _unitOfWork.CommitAsync();
            var conversationDto = _mapper.Map<ConversationDto>(conversation);
            return conversationDto;
        }
    }
}
