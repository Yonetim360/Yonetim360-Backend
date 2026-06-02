using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.Users.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public DeleteUserCommandHandler(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();
            var currentUserId = _currentUserService.GetUserId();

            if (request.Id == currentUserId)
                throw new InvalidDataException("Kendi kullanicinizi silemezsiniz.");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken)
                ?? throw new InvalidDataException("Kullanici bulunamadi.");

            var isLinkedToRepresentative = await _context.Representatives
                .AnyAsync(x => x.TenantId == tenantId && x.ApplicationUserId == user.Id, cancellationToken);

            if (isLinkedToRepresentative)
                throw new InvalidDataException("Temsilciye bagli kullanici silinemez. Once temsilci kaydini silin.");

            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidDataException(
                    $"Kullanici silinemedi: {string.Join(", ", result.Errors.Select(x => x.Description))}");
            }

            return true;
        }
    }
}
