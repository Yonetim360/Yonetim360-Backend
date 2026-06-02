using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO.CommonDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Users.AssignRolesToUser
{
    public class AssignRolesToUserCommand:ICommand<bool>
    {
        public Guid ApplicationUserId { get; set; }
        public List<RoleAssignmentDto> Roles { get; set; } = new();
    }
}
