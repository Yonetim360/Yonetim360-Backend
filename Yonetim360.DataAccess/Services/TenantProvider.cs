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
                var context = _httpContextAccessor.HttpContext;
                if (context == null)
                    throw new InvalidOperationException("HttpContext is not available");

                // Header'dan tenant ID al
                var header = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

                if (string.IsNullOrEmpty(header))
                    throw new UnauthorizedAccessException("Tenant ID is required");

                if (!Guid.TryParse(header, out var tenantId) || tenantId == Guid.Empty)
                    throw new UnauthorizedAccessException("Invalid Tenant ID format");

                return tenantId;
            }
        }

        //  exception fırlatmaz uygulama çökmez
        public bool TryGetTenantId(out Guid tenantId)
        {
            tenantId = Guid.Empty;

            var context = _httpContextAccessor.HttpContext;
            if (context == null) return false;

            var header = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();
            if (string.IsNullOrEmpty(header)) return false;

            return Guid.TryParse(header, out tenantId) && tenantId != Guid.Empty;
        }
    }
}

//TenantProvider, HTTP isteklerinde header'dan tenant ID'yi okuyup,
//bu ID'yi kullanılabilir hale getiren bir  servis sınıfıdır.
//Yani Gelen isteğin hangi müşteriye (tenanta) ait olduğunu anlamaya yarar