using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360Business.Services.Abstract
{
    public interface ICurrentUserService
    {
        Guid GetTenantId();
        Guid GetUserId();
        string? GetUserEmail();
    }
}
