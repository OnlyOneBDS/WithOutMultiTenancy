using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WithOutMultiTenancy.Application.Common.Caching;
using WithOutMultiTenancy.Application.Common.Events;
using WithOutMultiTenancy.Application.Common.Exceptions;
using WithOutMultiTenancy.Application.Common.FileStorage;
using WithOutMultiTenancy.Application.Common.Interfaces;
using WithOutMultiTenancy.Application.Common.Mailing;
using WithOutMultiTenancy.Application.Common.Models;
using WithOutMultiTenancy.Application.Common.Specification;
using WithOutMultiTenancy.Application.Identity.Users;
using WithOutMultiTenancy.Domain.Identity;
using WithOutMultiTenancy.Infrastructure.Auth;
using WithOutMultiTenancy.Infrastructure.Persistence.Context;
using WithOutMultiTenancy.Shared.Authorization;

namespace WithOutMultiTenancy.Infrastructure.Identity;

internal partial class UserService : IUserService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly RoleManager<ApplicationRole> _roleManager;
  private readonly ApplicationDbContext _context;
  private readonly SecuritySettings _securitySettings;
  private readonly IStringLocalizer _localizer;
  private readonly ICacheKeyService _cacheKeys;
  private readonly ICacheService _cache;
  private readonly IEmailTemplateService _templateService;
  private readonly IEventPublisher _events;
  private readonly IFileStorageService _fileStorage;
  private readonly IJobService _jobService;
  private readonly IMailService _mailService;

  public UserService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context,
    IOptions<SecuritySettings> securitySettings,
    IStringLocalizer<UserService> localizer,
    ICacheKeyService cacheKeys,
    ICacheService cache,
    IEmailTemplateService templateService,
    IEventPublisher events,
    IFileStorageService fileStorage,
    IJobService jobService,
    IMailService mailService)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _roleManager = roleManager;
    _context = context;
    _securitySettings = securitySettings.Value;
    _localizer = localizer;
    _cache = cache;
    _cacheKeys = cacheKeys;
    _templateService = templateService;
    _events = events;
    _fileStorage = fileStorage;
    _jobService = jobService;
    _mailService = mailService;
  }

  public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
  {
    var spec = new EntitiesByPaginationFilterSpec<ApplicationUser>(filter);

    var users = await _userManager.Users
      .WithSpecification(spec)
      .ProjectToType<UserDetailsDto>()
      .ToListAsync(cancellationToken);

    int count = await _userManager.Users
      .CountAsync(cancellationToken);

    return new PaginationResponse<UserDetailsDto>(users, count, filter.PageNumber, filter.PageSize);
  }

  public async Task<bool> ExistsWithNameAsync(string name)
  {
    return await _userManager.FindByNameAsync(name) is not null;
  }

  public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
  {
    return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
  }

  public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
  {
    return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
  }

  public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
  {
    return (await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken)).Adapt<List<UserDetailsDto>>();
  }

  public Task<int> GetCountAsync(CancellationToken cancellationToken)
  {
    return _userManager.Users.AsNoTracking().CountAsync(cancellationToken);
  }

  public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
  {
    var user = await _userManager.Users.AsNoTracking().Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken)
      ?? throw new NotFoundException(_localizer["User Not Found."]);

    return user.Adapt<UserDetailsDto>();
  }

  public async Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
  {
    var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken)
      ?? throw new NotFoundException(_localizer["User Not Found."]);

    bool isAdmin = await _userManager.IsInRoleAsync(user, FSHRoles.Admin);

    if (isAdmin)
    {
      throw new ConflictException(_localizer["Administrators Profile's Status cannot be toggled"]);
    }

    user.IsActive = request.ActivateUser;

    await _userManager.UpdateAsync(user);
    await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));
  }
}