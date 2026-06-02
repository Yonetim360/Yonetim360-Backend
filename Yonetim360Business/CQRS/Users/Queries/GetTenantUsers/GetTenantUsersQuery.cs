using Yonetim360Business.DTO.CommonDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.Users.Queries.GetTenantUsers
{
    public class GetTenantUsersQuery : IQuery<List<ApplicationUserDto>>
    {
        public bool OnlyUnlinkedRepresentatives { get; set; }
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;
    }
}
