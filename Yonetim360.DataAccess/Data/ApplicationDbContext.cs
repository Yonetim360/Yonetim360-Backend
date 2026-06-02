using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
using Yonetim360.Entity.User;

namespace Yonetim360.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly ITenantProvider _tenantProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<CustomerSupportRequest> CustomerSupportRequests { get; set; }
        public DbSet<CrmTask> Tasks { get; set; }
        public DbSet<CrmSolutionRequest> CrmSolutionRequests { get; set; }
        public DbSet<TokenBlacklist> TokenBlacklists { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementDepartment> AnnouncementDepartments { get; set; }
        public DbSet<AnnouncementRepresentative> AnnouncementRepresentatives { get; set; }
        public DbSet<WhatsAppMessage> WhatsAppMessages { get; set; }
        public DbSet<WhatsAppTemplate> WhatsAppTemplates { get; set; }
        public DbSet<WhatsAppSettings> WhatsAppSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WhatsAppMessage>()
                .HasOne(x => x.Template)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WhatsAppMessage>()
                .HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WhatsAppMessage>()
                .HasIndex(x => x.ProviderMessageSid);

            modelBuilder.Entity<WhatsAppMessage>()
                .HasIndex(x => new { x.TenantId, x.Status, x.ScheduledAt });

            modelBuilder.Entity<WhatsAppSettings>()
                .HasIndex(x => x.TenantId)
                .IsUnique();

            // Soft delete ve tenant filtreleri birlikte uygula
            ApplyCombinedFilters(modelBuilder);
        }



        private void ApplyCombinedFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                // BaseEntity'den miras alan ve ITenantEntity implement eden tipler için
                if (typeof(BaseEntity).IsAssignableFrom(clrType) && typeof(ITenantEntity).IsAssignableFrom(clrType))
                {
                    // Her iki filtreyi birleştir
                    var parameter = Expression.Parameter(clrType, "e");

                    // Soft delete filter: !e.IsDeleted
                    var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var softDeleteFilter = Expression.Not(isDeletedProperty);

                    // Tenant filter: e.TenantId == GetCurrentTenantId()
                    var tenantIdProperty = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
                    var currentTenantMethod = typeof(ApplicationDbContext)
                        .GetMethod(nameof(GetCurrentTenantId), BindingFlags.NonPublic | BindingFlags.Instance);
                    var currentTenantCall = Expression.Call(Expression.Constant(this), currentTenantMethod);
                    var tenantFilter = Expression.Equal(tenantIdProperty, currentTenantCall);

                    // İki filtreyi AND ile birleştir
                    var combinedFilter = Expression.AndAlso(softDeleteFilter, tenantFilter);
                    var lambdaExpression = Expression.Lambda(combinedFilter, parameter);

                    entityType.SetQueryFilter(lambdaExpression);
                }
                // Sadece BaseEntity'den miras alan tipler için (tenant olmayan)
                else if (typeof(BaseEntity).IsAssignableFrom(clrType))
                {
                    var parameter = Expression.Parameter(clrType, "e");
                    var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var softDeleteFilter = Expression.Not(isDeletedProperty);
                    var lambdaExpression = Expression.Lambda(softDeleteFilter, parameter);

                    entityType.SetQueryFilter(lambdaExpression);
                }
                // Sadece ITenantEntity implement eden tipler için (BaseEntity olmayan)
                else if (typeof(ITenantEntity).IsAssignableFrom(clrType))
                {
                    var parameter = Expression.Parameter(clrType, "e");
                    var tenantIdProperty = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
                    var currentTenantMethod = typeof(ApplicationDbContext)
                        .GetMethod(nameof(GetCurrentTenantId), BindingFlags.NonPublic | BindingFlags.Instance);
                    var currentTenantCall = Expression.Call(Expression.Constant(this), currentTenantMethod);
                    var tenantFilter = Expression.Equal(tenantIdProperty, currentTenantCall);
                    var lambdaExpression = Expression.Lambda(tenantFilter, parameter);

                    entityType.SetQueryFilter(lambdaExpression);
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
