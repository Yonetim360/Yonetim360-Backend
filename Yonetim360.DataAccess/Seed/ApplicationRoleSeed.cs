using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity;
using Yonetim360.Entity.User;

namespace Yonetim360.DataAccess.Seed
{
    public static class ApplicationRoleSeed
    {
        public static async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<ApplicationRole>
        {
            new ApplicationRole { Name = Roles.Representative, ModuleName = Modules.CRM },
            new ApplicationRole { Name = Roles.HR, ModuleName = Modules.HR },
            new ApplicationRole { Name = Roles.Personnel, ModuleName = Modules.HR },
            new ApplicationRole { Name = Roles.StockPersonnel, ModuleName = Modules.Stock },
            new ApplicationRole { Name = Roles.TenantAdmin, ModuleName = null } // Ortak rol
        };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                    await roleManager.CreateAsync(role);
            }
        }
    }
}
