using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WithOutMultiTenancy.Application.Common.Exceptions;
using WithOutMultiTenancy.Application.Identity.Tokens;
using WithOutMultiTenancy.Infrastructure.Auth;
using WithOutMultiTenancy.Infrastructure.Auth.Jwt;
using WithOutMultiTenancy.Shared.Authorization;

namespace WithOutMultiTenancy.Infrastructure.Identity;

internal class TokenService : ITokenService
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IStringLocalizer _localizer;
  private readonly SecuritySettings _securitySettings;
  private readonly JwtSettings _jwtSettings;

  public TokenService(UserManager<ApplicationUser> userManager, IStringLocalizer<TokenService> localizer, IOptions<SecuritySettings> securitySettings, IOptions<JwtSettings> jwtSettings)
  {
    _userManager = userManager;
    _localizer = localizer;
    _securitySettings = securitySettings.Value;
    _jwtSettings = jwtSettings.Value;
  }

  public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
  {
    if (await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user || !await _userManager.CheckPasswordAsync(user, request.Password))
    {
      throw new UnauthorizedException(_localizer["Authentication Failed."]);
    }

    if (!user.IsActive)
    {
      throw new UnauthorizedException(_localizer["User Not Active. Please contact the administrator."]);
    }

    if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
    {
      throw new UnauthorizedException(_localizer["Email not confirmed."]);
    }

    return await GenerateTokensAndUpdateUser(user, ipAddress);
  }

  public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
  {
    var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
    string? userEmail = userPrincipal.GetEmail();

    var user = await _userManager.FindByEmailAsync(userEmail!)
      ?? throw new UnauthorizedException(_localizer["Authentication failed"]);

    if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
      throw new UnauthorizedException(_localizer["Invalid refresh token"]);
    }

    return await GenerateTokensAndUpdateUser(user, ipAddress);
  }

  private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
  {
    string token = GenerateJwt(user, ipAddress);

    user.RefreshToken = GenerateRefreshToken();
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

    await _userManager.UpdateAsync(user);
    return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
  }

  private string GenerateJwt(ApplicationUser user, string ipAddress)
  {
    return GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
  }

  private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress)
  {
    return new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id),
      new(ClaimTypes.Email, user.Email!),
      new(FSHClaims.Fullname, $"{user.FirstName} {user.LastName}"),
      new(ClaimTypes.Name, user.FirstName ?? string.Empty),
      new(ClaimTypes.Surname, user.LastName ?? string.Empty),
      new(FSHClaims.IpAddress, ipAddress),
      new(FSHClaims.ImageUrl, user.ImageUrl ?? string.Empty),
      new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
    };
  }

  private static string GenerateRefreshToken()
  {
    byte[] randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);

    return Convert.ToBase64String(randomNumber);
  }

  private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
  {
    var token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
      signingCredentials: signingCredentials);

    var tokenHandler = new JwtSecurityTokenHandler();

    return tokenHandler.WriteToken(token);
  }

  private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
      ValidateIssuer = false,
      ValidateAudience = false,
      RoleClaimType = ClaimTypes.Role,
      ClockSkew = TimeSpan.Zero,
      ValidateLifetime = false
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

    if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
    {
      throw new UnauthorizedException(_localizer["Invalid Token."]);
    }

    return principal;
  }

  private SigningCredentials GetSigningCredentials()
  {
    byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
    return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
  }
}