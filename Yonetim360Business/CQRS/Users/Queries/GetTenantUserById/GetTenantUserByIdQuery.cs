using Yonetim360Business.DTO.CommonDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Users.Queries.GetTenantUserById
{
    public class GetTenantUserByIdQuery : IQuery<ApplicationUserDto>
    {
        public Guid Id { get; set; }
    }
}
