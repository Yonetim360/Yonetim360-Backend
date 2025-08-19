using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Authentication.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public LogoutCommandHandler(UserManager<ApplicationUser> userManager,
                                    ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null)
                throw new UnauthorizedAccessException("Geçersiz refresh token");

            // Refresh token blacklist’e ekle
            _context.TokenBlacklists.Add(new TokenBlacklist
            {
                Token = user.RefreshToken,
                Expiration = user.RefreshTokenEndDate ?? DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // Kullanıcının token bilgisini temizle
            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;
            await _userManager.UpdateAsync(user);

            return true;
        }
    }


}
