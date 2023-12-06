namespace WithOutMultiTenancy.Infrastructure.Persistence.Configuration;

internal static class SchemaNames
{
  public static string App = nameof(App);                   // "APPLICATION";
  public static string Auditing = nameof(Auditing);         // "AUDITING";
  public static string Identity = nameof(Identity);         // "IDENTITY";
  public static string MultiTenancy = nameof(MultiTenancy); // "MULTITENANCY";
}