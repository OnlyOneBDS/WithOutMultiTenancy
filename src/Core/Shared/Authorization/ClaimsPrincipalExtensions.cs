using WithOutMultiTenancy.Shared.Authorization;

namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
  public static string? GetUserId(this ClaimsPrincipal principal)
  {
    return principal.FindFirstValue(ClaimTypes.NameIdentifier);
  }

  public static string? GetSchoolId(this ClaimsPrincipal principal)
  {
    return principal?.FindFirst(FSHClaims.SchoolId)?.Value;
  }

  public static string? GetFirstName(this ClaimsPrincipal principal)
  {
    return principal?.FindFirst(ClaimTypes.Name)?.Value;
  }

  public static string? GetLastName(this ClaimsPrincipal principal)
  {
    return principal?.FindFirst(ClaimTypes.Surname)?.Value;
  }

  public static string? GetEmail(this ClaimsPrincipal principal)
  {
    return principal.FindFirstValue(ClaimTypes.Email);
  }

  public static string? GetPhoneNumber(this ClaimsPrincipal principal)
  {
    return principal.FindFirstValue(ClaimTypes.MobilePhone);
  }

  public static string? GetImageUrl(this ClaimsPrincipal principal)
  {
    return principal.FindFirstValue(FSHClaims.ImageUrl);
  }

  public static string? GetFullName(this ClaimsPrincipal principal)
  {
    return principal?.FindFirst(FSHClaims.Fullname)?.Value;
  }

  public static DateTimeOffset GetExpiration(this ClaimsPrincipal principal)
  {
    return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(principal.FindFirstValue(FSHClaims.Expiration)));
  }

  private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType)
  {
    return principal is null
      ? throw new ArgumentNullException(nameof(principal))
      : principal.FindFirst(claimType)?.Value;
  }
}