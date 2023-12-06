using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WithOutMultiTenancy.Application.Common.Exceptions;
using WithOutMultiTenancy.Infrastructure.Common;

namespace WithOutMultiTenancy.Infrastructure.Identity;

internal partial class UserService
{
  private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
  {
    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

    const string route = "api/users/confirm-email/";
    var endpointUri = new Uri(string.Concat($"{origin}/", route));
    string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);

    verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
    return verificationUri;
  }

  public async Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken)
  {
    var user = await _userManager.Users.Where(u => u.Id == userId && !u.EmailConfirmed).FirstOrDefaultAsync(cancellationToken)
      ?? throw new InternalServerException(_localizer["An error occurred while confirming email."]);

    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

    var result = await _userManager.ConfirmEmailAsync(user, code);

    return result.Succeeded
      ? string.Format(_localizer["Account confirmed for email {0}. You can now use the /api/tokens endpoint to generate JWT."], user.Email)
      : throw new InternalServerException(string.Format(_localizer["An error occurred while confirming {0}"], user.Email));
  }

  public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
  {
    var user = await _userManager.FindByIdAsync(userId)
      ?? throw new InternalServerException(_localizer["An error occurred while confirming phone number."]);

    if (string.IsNullOrEmpty(user.PhoneNumber))
    {
      throw new InternalServerException(_localizer["An error occurred while confirming phone number."]);
    }

    var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

    if (user.PhoneNumberConfirmed)
    {
      return result.Succeeded
        ? string.Format(_localizer["Account confirmed for phone number {0}. You can now use the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
        : throw new InternalServerException(string.Format(_localizer["An error occurred while confirming {0}"], user.PhoneNumber));
    }
    else
    {
      return result.Succeeded
        ? string.Format(_localizer["Account confirmed for phone number {0}. You should confirm your E-mail before using the /api/tokens endpoint to generate JWT."], user.PhoneNumber)
        : throw new InternalServerException(string.Format(_localizer["An error occurred while confirming {0}"], user.PhoneNumber));
    }
  }
}
