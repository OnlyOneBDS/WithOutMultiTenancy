using System.Security.Claims;

namespace WithOutMultiTenancy.Infrastructure.Auth;

public interface ICurrentUserInitializer
{
  void SetCurrentUser(ClaimsPrincipal user);
  void SetCurrentUserId(string userId);
}