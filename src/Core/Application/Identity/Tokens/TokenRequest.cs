namespace WithOutMultiTenancy.Application.Identity.Tokens;

public record TokenRequest(string Email, string Password);

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
  public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> T)
  {
    RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
      .NotEmpty()
      .EmailAddress()
        .WithMessage(T["Invalid email address."]);

    RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
      .NotEmpty();
  }
}