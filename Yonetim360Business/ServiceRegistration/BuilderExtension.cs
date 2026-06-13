using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Data;
using Yonetim360Business.BackgroundServices;
using Yonetim360Business.Behaviors;
using Yonetim360Business.CQRS.CRM.Customers.Commands.CreateCustomer;
using Yonetim360Business.CQRS.DocumentProcessing.Commands.CreateUploadedDocument;
using Yonetim360Business.CQRS.Users.CreateUser;
using Yonetim360Business.MappingProfile.CRM;
using Yonetim360.Entity.DocumentProcessing;
using Yonetim360Business.Services.Abstract;
using Yonetim360Business.Services.Concrete;
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
            services.AddValidatorsFromAssemblyContaining<CreateUploadedDocumentCommand>();
            services.AddAutoMapper(_ => { }, typeof(MappingConfig).Assembly);
            services.AddOptions<DocumentStorageOptions>();
            services.AddScoped<IAnnouncementHubService, AnnouncementHubService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDocumentStorageService, DocumentStorageService>();
            services.AddScoped<IDocumentOcrService, DocumentOcrService>();
            services.AddScoped<IDocumentClassificationService, DocumentClassificationService>();
            services.AddScoped<IPageGroupingService, PageGroupingService>();
            services.AddScoped<IPdfSplitService, PdfSplitService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICrmTaskAssignmentEmailHandler, CrmTaskAssignmentEmailHandler>();
            services.AddScoped<IWhatsAppService, WhatsAppService>();
            services.AddScoped<IWhatsAppMessageDispatcher, WhatsAppMessageDispatcher>();
            services.AddHostedService<WhatsAppReminderWorker>();
            services.AddHostedService<WhatsAppStatusSyncWorker>();
            return services;

        }
    }
}
