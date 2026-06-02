using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.Users.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _notificationService;

        public CreateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ICurrentUserService currentUserService,
            IEmailService notificationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                TenantId = tenantId
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new InvalidDataException(
                    $"Kullanici olusturulamadi: {string.Join(", ", result.Errors.Select(x => x.Description))}");
            }

            if (request.RoleIds?.Any() == true)
            {
                var roles = await _roleManager.Roles
                    .Where(x => request.RoleIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                foreach (var role in roles)
                {
                    if (!string.IsNullOrWhiteSpace(role.Name))
                        await _userManager.AddToRoleAsync(user, role.Name);
                }
            }

            var subject = "ERP Sistemine Giris Bilgileriniz";
            var body = $@"
                <p>Merhaba {user.FirstName},</p>
                <p>Sisteme giris sifreniz: <b>{request.Password}</b></p>
                <p>ERP paneline giris yapmak icin <a href='https://app.yonetim360.com'>buraya</a> tiklayabilirsiniz.</p>
                <p>Iyi calismalar dileriz.</p>";

            await _notificationService.SendMailAsync(user.Email, subject, body);

            return true;
        }
    }
}
