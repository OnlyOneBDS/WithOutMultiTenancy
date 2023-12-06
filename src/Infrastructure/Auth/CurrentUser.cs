using System.Security.Claims;
using WithOutMultiTenancy.Application.Common.Interfaces;

namespace WithOutMultiTenancy.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
  private ClaimsPrincipal? _user;

  public string? Name
  {
    get
    {
      return _user?.Identity?.Name;
    }
  }

  private Guid _userId = Guid.Empty;

  public Guid GetUserId()
  {
    return IsAuthenticated()
      ? Guid.Parse(_user?.GetUserId() ?? Guid.Empty.ToString())
      : _userId;
  }

  public string? GetUserEmail()
  {
    return IsAuthenticated()
      ? _user!.GetEmail()
      : string.Empty;
  }

  public bool IsAuthenticated() =>
      _user?.Identity?.IsAuthenticated is true;

  public bool IsInRole(string role)
  {
    return _user?.IsInRole(role) is true;
  }

  public IEnumerable<Claim>? GetUserClaims()
  {
    return _user?.Claims;
  }

  public void SetCurrentUser(ClaimsPrincipal user)
  {
    if (_user != null)
    {
      throw new Exception("Method reserved for in-scope initialization");
    }

    _user = user;
  }

  public void SetCurrentUserId(string userId)
  {
    if (_userId != Guid.Empty)
    {
      throw new Exception("Method reserved for in-scope initialization");
    }

    if (!string.IsNullOrEmpty(userId))
    {
      _userId = Guid.Parse(userId);
    }
  }
}