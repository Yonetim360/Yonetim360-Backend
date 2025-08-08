using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Extensions;
using Yonetim360.DataAccess.Services;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;

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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<CustomerSupportRequest> CustomerSupportRequests { get; set; }
        public DbSet<CrmTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Soft delete filter
            modelBuilder.ApplySoftDeleteFilter();

            // Tenant filters - DİNAMİK OLARAK
            ApplyTenantFilters(modelBuilder);
        }

        private void ApplyTenantFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Expression dinamik oluştur
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var tenantIdProperty = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
                    var tenantIdConstant = Expression.Constant(null, typeof(Guid?));

                    // Runtime'da değerlendirilecek expression
                    var currentTenantMethod = typeof(ApplicationDbContext)
                        .GetMethod(nameof(GetCurrentTenantId), BindingFlags.NonPublic | BindingFlags.Instance);
                    var currentTenantCall = Expression.Call(Expression.Constant(this), currentTenantMethod);

                    var equalExpression = Expression.Equal(tenantIdProperty, currentTenantCall);
                    var lambdaExpression = Expression.Lambda(equalExpression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambdaExpression);
                }
            }
        }

        private Guid GetCurrentTenantId()
        {
            // runtimeda çağrılır
            return _tenantProvider.TenantId;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Tenant ID kontrolü ve ataması
            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.TenantId = _tenantProvider.TenantId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Tenant ID değiştirilmesin
                    entry.Property(x => x.TenantId).IsModified = false;
                }
            }

            // Soft delete işlemleri
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
