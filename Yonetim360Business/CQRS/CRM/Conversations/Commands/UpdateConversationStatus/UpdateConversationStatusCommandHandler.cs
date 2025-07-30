using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversationStatus
{
    public class UpdateConversationStatusCommandHandler : ICommandHandler<UpdateConversationStatusCommand, bool>
    {
        private readonly IRepository<Conversation> _conversationReository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateConversationStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _conversationReository = _unitOfWork.GetRepository<Conversation>();
        }

        public async Task<bool> Handle(UpdateConversationStatusCommand request, CancellationToken cancellationToken)
        {
           var conversation = await _conversationReository.GetFirstOrDefaultAsync(x=>x.Id==request.ConversationId) ?? 
                throw new InvalidDataException("Conversation not found");
            conversation.ConversationStatus = request.NewStatus;
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
