using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.İK
{
    public class Department:BaseEntity,ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
    }
}
