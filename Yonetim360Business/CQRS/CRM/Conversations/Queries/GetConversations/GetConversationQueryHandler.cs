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

namespace Yonetim360Business.CQRS.CRM.Conversations.Queries.GetConversations
{
    public class GetConversationQueryHandler : IQueryHandler<GetConversationQuery, List<ConversationDto>>
    {
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetConversationQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _conversationRepository = _unitOfWork.GetRepository<Conversation>();
        }

        public async Task<List<ConversationDto>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _conversationRepository.GetAllAsync(
                filter: c =>
               (!request.CustomerId.HasValue || c.CustomerId == request.CustomerId.Value) &&
               (!request.ConversationStatus.HasValue || c.ConversationStatus == request.ConversationStatus.Value),
                tracked: false,
                pageSize: request.PageSize,
                pageNumber: request.PageNumber,
                include: q => q
                .Include(c => c.Representatives)
                .Include(c => c.Customer))
                             
                ?? throw new InvalidDataException("Not found any conversation records");

            return _mapper.Map<List<ConversationDto>>(conversations);
        }
    }
}
