using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation;
using Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer;
using Yonetim360Business.DTO;

namespace Yonetim360Business.MappingProfile.CRM
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateCustomerCommand, Customer>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<CreateConversationCommand, Conversation>().ReverseMap();
            CreateMap<ConversationDto, Conversation>().ReverseMap();
        }
    }
}
