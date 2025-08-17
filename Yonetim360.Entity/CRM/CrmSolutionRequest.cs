using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class CrmSolutionRequest:BaseEntity,ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public SolutionRequestType SolutionRequestType { get; set; }
        public string Title { get; set; }
        public Priority Piority { get; set; }
        public Module Module { get; set; }
        public string Detail { get; set; }
        public string Email { get; set; }
        public string PhoneNumber  { get; set; }
        public ConversationType ConversationType { get; set; }
        public string DocumentUrl { get; set; }




    }
    public enum SolutionRequestType
    {
        TecnicalSupport=0,
        UsageHelp=1,
        FeatureRequest=2,
        Performanceİmprovement=3,
        Other=4
    }
    public enum Module
    {
        CRM=0,
        İK=1,
        STOCK=2,
    }


}
