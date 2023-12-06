using System.Collections.ObjectModel;

namespace WithOutMultiTenancy.Shared.Authorization;

public static class FSHRoles
{
  public const string Admin = nameof(Admin);
  public const string Manager = nameof(Manager);
  public const string Contributor = nameof(Contributor);
  public const string Basic = nameof(Basic);

  public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
  {
    Admin,
    Manager,
    Contributor,
    Basic
  });

  public static bool IsDefault(string roleName)
  {
    return DefaultRoles.Any(r => r == roleName);
  }
}