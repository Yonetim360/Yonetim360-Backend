using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Users.UpdateUser
{
    public class UpdateUserCommand : ICommand<bool>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Password { get; set; }
    }
}
