using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.User
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? ModuleName { get; set; } //CRM , HR , STOCK

    }
}
