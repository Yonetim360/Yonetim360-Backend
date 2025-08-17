using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.DataAccess.Services
{
    public interface ITenantContextAccessor
    {
        Guid? CurrentTenantId { get; }
        IDisposable Use(Guid TenantId);
    }
}
