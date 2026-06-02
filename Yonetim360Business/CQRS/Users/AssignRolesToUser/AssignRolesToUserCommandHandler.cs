using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity;
using Yonetim360.Entity.User;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Users.AssignRolesToUser
{
    public class AssignRolesToUserCommandHandler : ICommandHandler<AssignRolesToUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly Dictionary<string, List<string>> _assignableRoles = new()
    {
        { Modules.CRM, new List<string> { Roles.Representative, Roles.TenantAdmin } },
        { Modules.HR, new List<string> { Roles.HR, Roles.Personnel, Roles.TenantAdmin } },
        { Modules.Stock, new List<string> { Roles.StockPersonnel, Roles.TenantAdmin } }
    };

        public AssignRolesToUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(AssignRolesToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.ApplicationUserId.ToString());
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            foreach (var roleDto in request.Roles)
            {
                var roleName = roleDto.RoleName;

                // Atama yetkisini kontrol et
                if (roleDto.ModuleName != null && !_assignableRoles.ContainsKey(roleDto.ModuleName))
                    throw new Exception($"Module {roleDto.ModuleName} tanımlı değil.");

                if (roleDto.ModuleName != null && !_assignableRoles[roleDto.ModuleName].Contains(roleName))
                    throw new Exception($"Kullanıcı bu role atanamaz: {roleName} için modül {roleDto.ModuleName}");

                var existingRole = await _roleManager.FindByNameAsync(roleName);
                if (existingRole == null)
                {
                    var newRole = new ApplicationRole
                    {
                        Name = roleName,
                        ModuleName = roleDto.ModuleName
                    };
                    await _roleManager.CreateAsync(newRole);
                }

                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Rol atama başarısız: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            return true;
        }
    }

}
