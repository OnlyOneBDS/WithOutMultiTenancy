using Microsoft.AspNetCore.WebUtilities;
using WithOutMultiTenancy.Application.Common.Exceptions;
using WithOutMultiTenancy.Application.Common.Mailing;
using WithOutMultiTenancy.Application.Identity.Users.Password;

namespace WithOutMultiTenancy.Infrastructure.Identity;

internal partial class UserService
{
  public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
  {
    var user = await _userManager.FindByEmailAsync(request.Email.Normalize());

    if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
    {
      // Don't reveal that the user does not exist or is not confirmed
      throw new InternalServerException(_localizer["An error has occurred!"]);
    }

    // For more information on how to enable account confirmation and password reset please
    // visit https://go.microsoft.com/fwlink/?LinkID=532713
    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
    const string route = "account/reset-password";
    var endpointUri = new Uri(string.Concat($"{origin}/", route));
    string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);

    var mailRequest = new MailRequest(
      new List<string> { request.Email },
      _localizer["Reset Password"],
      _localizer[$"Your password reset token is '{code}'. You can reset your password using the {endpointUri} endpoint."]);

    _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    return _localizer["Password reset email has been sent to your authorized email."];
  }

  public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
  {
    var user = await _userManager.FindByEmailAsync(request.Email?.Normalize()!)
      // Don't reveal that the user does not exist
      ?? throw new InternalServerException(_localizer["An Error has occurred!"]);

    var result = await _userManager.ResetPasswordAsync(user, request.Token!, request.Password!);

    return result.Succeeded
      ? _localizer["Password reset successful!"]
      : throw new InternalServerException(_localizer["An Error has occurred!"]);
  }

  public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
  {
    var user = await _userManager.FindByIdAsync(userId)
      ?? throw new NotFoundException(_localizer["User not found."]);

    var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

    if (!result.Succeeded)
    {
      throw new InternalServerException(_localizer["Change password failed"], result.GetErrors(_localizer));
    }
  }
}