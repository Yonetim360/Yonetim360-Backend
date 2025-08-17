using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Data;
using Yonetim360.DataAccess.Services;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Concrete;

namespace Yonetim360.DataAccess.Extensions
{
    public static class BuilderExtension
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ITenantContextAccessor, TenantContextAccessor>();
            return services;
        }
    }
}
