
using Yonetim360.DataAccess.Extensions;
using Yonetim360Business.ServiceRegistration;

namespace Yonetim360.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDataAccessServices(builder.Configuration);
            builder.Services.AddBusinessLayer();
            builder.Services.AddControllers();
            builder.Services.AddCors(options => options.AddPolicy("myclients", builder => builder.WithOrigins
            ("http://localhost:3000", "https://localhost:3000").AllowAnyMethod().AllowAnyHeader()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("myclients");
            app.UseHttpsRedirection();
            app.UseMiddleware<Yonetim360.API.Middlewares.TenantMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
