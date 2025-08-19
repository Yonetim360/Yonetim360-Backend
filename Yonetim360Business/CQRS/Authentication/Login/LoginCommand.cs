using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Authentication.Login
{
    public class LoginCommand:ICommand<LoginResultDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
