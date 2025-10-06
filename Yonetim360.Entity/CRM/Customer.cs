using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class Customer:BaseEntity,ITenantEntity
    {
        public Guid TenantId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber  { get; set; }
        public Segment Segment { get; set; }
        public State State { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

    }

   public enum Segment
    {
        institutional=1,
        kobi=2,
        individual=3
    }

    public enum State
    {
        Active = 1,
        Passive = 2,
        Potential = 3
    }
}
