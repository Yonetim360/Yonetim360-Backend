using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.DataAccess.Services
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid TenantId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null)
                    throw new InvalidOperationException("HttpContext veya kullanıcı bulunamadı");

                var tenantClaim = user.FindFirst("TenantId")?.Value;
                if (string.IsNullOrEmpty(tenantClaim) || !Guid.TryParse(tenantClaim, out var tenantId))
                    throw new UnauthorizedAccessException("Tenant ID geçersiz");

                return tenantId;
            }
        }

        public bool TryGetTenantId(out Guid tenantId)
        {
            tenantId = Guid.Empty;
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return false;

            var tenantClaim = user.FindFirst("TenantId")?.Value;
            return tenantClaim != null && Guid.TryParse(tenantClaim, out tenantId);
        }
    }
    }

//TenantProvider, HTTP isteklerinde header'dan tenant ID'yi okuyup,
//bu ID'yi kullanılabilir hale getiren bir  servis sınıfıdır.
//Yani Gelen isteğin hangi müşteriye (tenanta) ait olduğunu anlamaya yarar