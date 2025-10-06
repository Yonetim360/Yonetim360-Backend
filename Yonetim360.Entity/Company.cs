using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity
{
    public class Company:BaseEntity,ITenantEntity
    {
        public string Name { get; set; }
        public Guid TenantId { get; set; }
        public string? TwilioNumber { get; set; }

    }
}
