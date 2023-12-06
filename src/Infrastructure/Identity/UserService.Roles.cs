using Microsoft.EntityFrameworkCore;
using WithOutMultiTenancy.Application.Common.Exceptions;
using WithOutMultiTenancy.Application.Identity.Users;
using WithOutMultiTenancy.Domain.Identity;
using WithOutMultiTenancy.Shared.Authorization;

namespace WithOutMultiTenancy.Infrastructure.Identity;

internal partial class UserService
{
  public async Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken)
  {
    var userRoles = new List<UserRoleDto>();

    var user = await _userManager.FindByIdAsync(userId)
      ?? throw new NotFoundException("User Not Found.");

    var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken)
      ?? throw new NotFoundException("Roles Not Found.");

    foreach (var role in roles)
    {
      userRoles.Add(new UserRoleDto
      {
        RoleId = role.Id,
        RoleName = role.Name,
        Description = role.Description,
        Enabled = await _userManager.IsInRoleAsync(user, role.Name!)
      });
    }

    return userRoles;
  }

  public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request, nameof(request));

    var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken)
      ?? throw new NotFoundException(_localizer["User not found"]);

    // Check if the user is an admin for which the admin role is getting disabled
    if (await _userManager.IsInRoleAsync(user, FSHRoles.Admin) && request.UserRoles.Any(a => !a.Enabled && a.RoleName == FSHRoles.Admin))
    {
      // Get count of users in Admin Role
      int adminCount = (await _userManager.GetUsersInRoleAsync(FSHRoles.Admin)).Count;
            
      if (adminCount <= 2)
      {
        throw new ConflictException(_localizer["There should be at least 2 Admins."]);
      }
    }

    foreach (var userRole in request.UserRoles)
    {
      // Check if Role Exists
      if (await _roleManager.FindByNameAsync(userRole.RoleName!) is not null)
      {
        if (userRole.Enabled)
        {
          if (!await _userManager.IsInRoleAsync(user, userRole.RoleName!))
          {
            await _userManager.AddToRoleAsync(user, userRole.RoleName!);
          }
        }
        else
        {
          await _userManager.RemoveFromRoleAsync(user, userRole.RoleName!);
        }
      }
    }

    await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));
    return _localizer["User Roles Updated Successfully."];
  }
}