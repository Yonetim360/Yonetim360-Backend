using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

namespace Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversationById
{
    public class GetConversationByIdQueryHandler : IQueryHandler<GetConversationByIdQuery, ConversationDto>
    {
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IMapper  _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetConversationByIdQueryHandler( IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _conversationRepository = _unitOfWork.GetRepository<Conversation>();
            _mapper = mapper;
            
        }

        public async Task<ConversationDto> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetFirstOrDefaultAsync(
                filter: x => x.Id == request.Id,
                include: q => q
                .Include(c => c.Representatives)
                .Include(c=>c.Customer)) ?? throw new InvalidDataException("Not found a conversation record");

            return _mapper.Map<ConversationDto>(conversation);

        }
    }
}
