using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;
using System.IdentityModel.Tokens.Jwt;
using Yonetim360.Entity.User;

namespace Yonetim360Business.CQRS.Profiles.Queries.GetCurrentProfile
{
    public class GetCurrentProfileQueryHandler : IQueryHandler<GetCurrentProfileQuery, CurrentUserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrentProfileQueryHandler(UserManager<ApplicationUser> userManager,
                                          IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CurrentUserDto> Handle(GetCurrentProfileQuery request, CancellationToken cancellationToken)
        {
            var userId =
                _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                _httpContextAccessor.HttpContext.User.FindFirstValue("sub");
            if (userId == null)
                throw new UnauthorizedAccessException("Kullanıcı bulunamadı");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("Kullanıcı bulunamadı");

            return new CurrentUserDto
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,

            };
        }
    }
}
