using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity
{
    public interface ITenantEntity
    {
        public Guid TenantId { get; set; }
    }
}
