using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.Services.Concrete
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetTenantId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("TenantId")?.Value;
            return Guid.Parse(claim ?? throw new UnauthorizedAccessException("Tenant bilgisi bulunamadı."));
        }

        public Guid GetUserId()
        {
            var claim =
                _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            return Guid.Parse(claim ?? throw new UnauthorizedAccessException("Kullanıcı ID bilgisi bulunamadı."));
        }

        public string? GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
