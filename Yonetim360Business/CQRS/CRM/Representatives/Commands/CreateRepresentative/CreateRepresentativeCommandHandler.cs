using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity;
using Yonetim360.Entity.CRM;
using Yonetim360.Entity.User;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;
using Yonetim360Business.Services.Abstract;

namespace Yonetim360Business.CQRS.CRM.Representatives.Commands.CreateRepresentative
{
    public class CreateRepresentativeCommandHandler : ICommandHandler<CreateRepresentativeCommand, RepresentativeDto>
    {
        private readonly IRepository<Representative> _representativeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;

        public CreateRepresentativeCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _representativeRepository = _unitOfWork.GetRepository<Representative>();
        }

        public async Task<RepresentativeDto> Handle(CreateRepresentativeCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserService.GetTenantId();
            var currentUserId = _currentUserService.GetUserId();

            var selectedUser = await _userManager.FindByIdAsync(request.ApplicationUserId.ToString())
                ?? throw new InvalidDataException("ApplicationUser not found.");

            if (selectedUser.TenantId != tenantId)
                throw new UnauthorizedAccessException("Selected user does not belong to this tenant.");

            var existingRepresentative = await _representativeRepository.GetFirstOrDefaultAsync(
                x => x.ApplicationUserId == request.ApplicationUserId,
                tracked: false);

            if (existingRepresentative != null)
                throw new InvalidDataException("Selected user is already linked to a representative.");

            if (!await _roleManager.RoleExistsAsync(Roles.Representative))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = Roles.Representative,
                    ModuleName = Modules.CRM
                });
            }

            if (!await _userManager.IsInRoleAsync(selectedUser, Roles.Representative))
            {
                var roleResult = await _userManager.AddToRoleAsync(selectedUser, Roles.Representative);
                if (!roleResult.Succeeded)
                {
                    throw new InvalidDataException(
                        $"Representative role could not be assigned: {string.Join(", ", roleResult.Errors.Select(x => x.Description))}");
                }
            }

            var representative = _mapper.Map<Representative>(request);
            representative.Id = Guid.NewGuid();
            representative.ApplicationUserId = selectedUser.Id;
            representative.CreatedAt = DateTime.UtcNow;
            representative.CreatedBy = currentUserId;

            await _representativeRepository.CreateAsync(representative);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<RepresentativeDto>(representative)
                ?? throw new InvalidDataException("Representative could not be created.");
        }
    }
}
