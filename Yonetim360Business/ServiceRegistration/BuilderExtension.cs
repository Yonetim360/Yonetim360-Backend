using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.Behaviors;
using Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer;
using Yonetim360Business.CQRS.Users.CreateUser;
using Yonetim360Business.MappingProfile.CRM;
using Yonetim360Business.SignalR.Services;

namespace Yonetim360Business.ServiceRegistration
{
    public static class BuilderExtension
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssemblyContaining<CreateCustomerCommand>();
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IAnnouncementHubService, AnnouncementHubService>();

            return services;

        }
    }
}
