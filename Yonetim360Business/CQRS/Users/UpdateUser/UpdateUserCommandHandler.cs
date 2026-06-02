using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.Users.UpdateUser
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserCommandHandler(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken)
                ?? throw new InvalidDataException("Kullanici bulunamadi.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new InvalidDataException(
                    $"Kullanici guncellenemedi: {string.Join(", ", updateResult.Errors.Select(x => x.Description))}");
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, request.Password);
                if (!passwordResult.Succeeded)
                {
                    throw new InvalidDataException(
                        $"Sifre guncellenemedi: {string.Join(", ", passwordResult.Errors.Select(x => x.Description))}");
                }
            }

            var representative = await _context.Representatives
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.ApplicationUserId == user.Id, cancellationToken);

            if (representative != null)
            {
                representative.FirstName = request.FirstName;
                representative.LastName = request.LastName;
                representative.Email = request.Email;
                representative.PhoneNumber = request.PhoneNumber;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}
