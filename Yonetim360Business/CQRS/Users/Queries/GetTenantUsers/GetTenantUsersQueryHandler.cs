using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.User;
using Yonetim360Business.DTO.CommonDtos;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.Users.Queries.GetTenantUsers
{
    public class GetTenantUsersQueryHandler : IQueryHandler<GetTenantUsersQuery, List<ApplicationUserDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetTenantUsersQueryHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<List<ApplicationUserDto>> Handle(GetTenantUsersQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();
            var pageSize = request.PageSize <= 0 ? 100 : Math.Min(request.PageSize, 100);
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;

            var linkedRepresentativeUserIds = await _context.Representatives
                .AsNoTracking()
                .Where(x => x.TenantId == tenantId)
                .Select(x => x.ApplicationUserId)
                .ToListAsync(cancellationToken);

            var query = _userManager.Users
                .AsNoTracking()
                .Where(x => x.TenantId == tenantId);

            if (request.OnlyUnlinkedRepresentatives)
                query = query.Where(x => !linkedRepresentativeUserIds.Contains(x.Id));

            var users = await query
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var result = new List<ApplicationUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new ApplicationUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList(),
                    IsRepresentative = linkedRepresentativeUserIds.Contains(user.Id)
                });
            }

            return result;
        }
    }
}
