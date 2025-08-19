using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Authentication.Logout
{
    public class LogoutCommand : ICommand<bool>
    {
        public string RefreshToken { get; set; }
    }
}
