using Microsoft.AspNetCore.Identity;
using WithOutMultiTenancy.Application.Common.Events;
using WithOutMultiTenancy.Application.Identity.Users;
using WithOutMultiTenancy.Domain.Identity;

namespace WithOutMultiTenancy.Infrastructure.Identity;

internal class InvalidateUserPermissionCacheHandler :
  IEventNotificationHandler<ApplicationUserUpdatedEvent>,
  IEventNotificationHandler<ApplicationRoleUpdatedEvent>
{
  private readonly IUserService _userService;
  private readonly UserManager<ApplicationUser> _userManager;

  public InvalidateUserPermissionCacheHandler(UserManager<ApplicationUser> userManager)
  {
    _userManager = userManager;
  }

  public async Task Handle(EventNotification<ApplicationUserUpdatedEvent> notification, CancellationToken cancellationToken)
  {
    if (notification.Event.RolesUpdated)
    {
      await _userService.InvalidatePermissionCacheAsync(notification.Event.UserId, cancellationToken);
    }
  }

  public async Task Handle(EventNotification<ApplicationRoleUpdatedEvent> notification, CancellationToken cancellationToken)
  {
    if (notification.Event.PermissionsUpdated)
    {
      foreach (var user in await _userManager.GetUsersInRoleAsync(notification.Event.RoleName))
      {
        await _userService.InvalidatePermissionCacheAsync(user.Id, cancellationToken);
      }
    }
  }
}