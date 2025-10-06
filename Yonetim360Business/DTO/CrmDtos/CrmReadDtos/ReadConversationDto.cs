using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO.CrmReadDtos
{
    public class ReadConversationDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerCompanyName { get; set; }
        public ConversationType? ConversationType { get; set; }
        public MeetingType? MeetingType { get; set; }
        public string? ConversationInformation { get; set; }
        public string Subject { get; set; }
        public DateTime StartDateTime { get; set; }
        public int DurationInMinutes { get; set; }
        public List<string> RepresentativeNames { get; set; }
        public ConversationStatus? ConversationStatus { get; set; }
    }
}
