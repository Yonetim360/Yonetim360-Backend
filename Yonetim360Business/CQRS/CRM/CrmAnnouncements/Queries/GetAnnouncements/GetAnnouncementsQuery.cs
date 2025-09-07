using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmAnnouncements.Queries.GetAnnouncements
{
    public class GetAnnouncementsQuery:IQuery<List<AnnouncementDto>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } 
    }
}
