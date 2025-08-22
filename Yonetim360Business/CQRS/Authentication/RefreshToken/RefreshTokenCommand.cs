using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Authentication.RefreshToken
{
    public class RefreshTokenCommand : ICommand<LoginResultDto>
    {
        public string RefreshToken { get; set; }
    }
}
