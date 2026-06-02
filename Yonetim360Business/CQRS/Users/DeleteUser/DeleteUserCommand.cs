using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Users.DeleteUser
{
    public class DeleteUserCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
    }
}
