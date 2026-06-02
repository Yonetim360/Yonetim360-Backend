using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yonetim360.DataAccess.Data;
using Yonetim360.Entity.User;
using Yonetim360Business.DTO.CommonDtos;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.Users.Queries.GetTenantUserById
{
    public class GetTenantUserByIdQueryHandler : IQueryHandler<GetTenantUserByIdQuery, ApplicationUserDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetTenantUserByIdQueryHandler(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<ApplicationUserDto> Handle(GetTenantUserByIdQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken)
                ?? throw new InvalidDataException("Kullanici bulunamadi.");

            var isRepresentative = await _context.Representatives
                .AsNoTracking()
                .AnyAsync(x => x.TenantId == tenantId && x.ApplicationUserId == user.Id, cancellationToken);

            var roles = await _userManager.GetRolesAsync(user);

            return new ApplicationUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                IsRepresentative = isRepresentative
            };
        }
    }
}
