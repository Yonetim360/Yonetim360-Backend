using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;
using Yonetim360Business.CQRS.CommonModule.Announcements.Commands.CreateAnnouncement;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.CreateConversation;
using Yonetim360Business.CQRS.CRM.Conversations.Commands.UpdateConversationStatus;
using Yonetim360Business.CQRS.CRM.CrmAnnouncements.Commands.AssignAnnouncementToRepresentatives;
using Yonetim360Business.CQRS.CRM.CrmSolutionCenters.Commands.CreateSolutionRequest;
using Yonetim360Business.CQRS.CRM.CrmTasks.Commands.CreateCrmTask;
using Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer;
using Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Commands.CreateCustomerSupportRequest;
using Yonetim360Business.CQRS.CRM.OfferAndSales.Commands.CreateOffer;
using Yonetim360Business.CQRS.CRM.Representatives.Commands.CreateRepresentative;
using Yonetim360Business.DTO;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.DTO.CrmReadDtos;

namespace Yonetim360Business.MappingProfile.CRM
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CreateCustomerCommand, Customer>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<Customer, ReadCustomerDto>();
            CreateMap<CreateConversationCommand, Conversation>().ReverseMap();
            CreateMap<Conversation, ReadConversationDto>()
           .ForMember(dest => dest.CustomerCompanyName,
               opt => opt.MapFrom(src => src.Customer.CompanyName))
           .ForMember(dest=>dest.RepresentativeNames,
               opt=>opt.MapFrom(src=>src.Representatives.Select(r=>r.FirstName + " " + r.LastName).ToList())
           );
            CreateMap<Conversation,ConversationDto>().ReverseMap();
            CreateMap<UpdateConversationStatusCommand, Conversation>().ReverseMap();
            CreateMap<Offer,ReadOfferDto>()
                .ForMember(dest=>dest.CustomerCompanyName,opt=>opt.MapFrom(src=>src.Customer.CompanyName))
                .ForMember(dest=>dest.RepresentativeName,opt=>opt.MapFrom(src=>src.Representative.FirstName + " " + src.Representative.LastName));
            CreateMap<Offer, OfferDto>().ReverseMap();
            CreateMap<CreateOfferCommand, Offer>().ReverseMap();
            CreateMap<CreateRepresentativeCommand, Representative>().ReverseMap();
            CreateMap<RepresentativeDto,Representative>().ReverseMap();
            CreateMap<Representative, ReadRepresentativeDto>();
            CreateMap<CreateCustomerSupportRequestCommand,CustomerSupportRequest>().ReverseMap();
            CreateMap<CustomerSupportRequestDto,CustomerSupportRequest>().ReverseMap();
            CreateMap<CustomerSupportRequest, ReadCustomerSupportRequestDto>()
                .ForMember(dest => dest.CustomerCompanyName, opt => opt.MapFrom(src => src.Customer.CompanyName));
            CreateMap<CreateCrmTaskCommand, CrmTask>().ReverseMap();
            CreateMap<CrmTaskDto, CrmTask>().ReverseMap();
            CreateMap<CrmTask, ReadCrmTaskDto>();
            CreateMap<CrmSolutionRequestDto, CrmSolutionRequest>().ReverseMap();
            CreateMap<CrmSolutionRequest, ReadCrmSolutionRequestDto>();
            CreateMap<CreateCrmSolutionRequestCommand, CrmSolutionRequest>().ReverseMap();
            CreateMap<CreateAnnouncementCommand,Announcement>().ReverseMap();
            CreateMap<AssignAnnouncementToRepresentativeCommand,AnnouncementRepresentative>().ReverseMap();
            CreateMap<AnnouncementDto, Announcement>().ReverseMap();
            CreateMap<Announcement, ReadAnnouncementDto>();
            



        }
    }
}
