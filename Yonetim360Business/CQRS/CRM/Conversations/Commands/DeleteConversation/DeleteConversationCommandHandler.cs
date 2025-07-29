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

namespace Yonetim360Business.CQRS.CRM.Conversations.Commands.DeleteConversation
{
    public class DeleteConversationCommandHandler : ICommandHandler<DeleteConversationCommand, bool>
    {
        private readonly IRepository<Conversation> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteConversationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Conversation>();

        }

        public async Task<bool> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
        {
           var user = await _repository.GetFirstOrDefaultAsync(x=>x.Id== request.Id)?? throw new Exception("Conversation not found");
            await _repository.DeleteAsync(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
