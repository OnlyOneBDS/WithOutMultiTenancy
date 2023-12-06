using Microsoft.AspNetCore.Authorization;
using WithOutMultiTenancy.Shared.Authorization;

namespace WithOutMultiTenancy.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
  public MustHavePermissionAttribute(string action, string resource)
  {
    Policy = FSHPermission.NameFor(action, resource);
  }
}