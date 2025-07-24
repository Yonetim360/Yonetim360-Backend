using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Services;
using Yonetim360.Entity;

namespace Yonetim360.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tüm tenant entity'ler için otomatik filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                        .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Static)
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder, _tenantProvider });
                }
            }
        }

        private static void SetTenantFilter<TEntity>(ModelBuilder modelBuilder, ITenantProvider tenantProvider)
            where TEntity : class, ITenantEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == tenantProvider.TenantId);
        }
    }
}
