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
                var header = _httpContextAccessor.HttpContext?.Request?.Headers["X-Tenant-ID"];

                if (!string.IsNullOrEmpty(header))
                {
                    if (Guid.TryParse(header, out var tenantId))
                    {
                        return tenantId;
                    }
                }

                return Guid.Empty;
            }
        }


    }
}

//TenantProvider, HTTP isteklerinde header'dan tenant ID'yi okuyup,
//bu ID'yi kullanılabilir hale getiren bir  servis sınıfıdır.
//Yani Gelen isteğin hangi müşteriye (tenanta) ait olduğunu anlamaya yarar