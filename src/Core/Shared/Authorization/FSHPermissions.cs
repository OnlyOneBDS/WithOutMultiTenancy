using System.Collections.ObjectModel;

namespace WithOutMultiTenancy.Shared.Authorization;

public static class FSHPermissions
{
  private static readonly FSHPermission[] _all = new FSHPermission[]
  {
    // Admin stuff
    new("View Dashboard", FSHAction.View, FSHResource.Dashboard, IsAdmin: true, IsManager: true, IsContributor: true),
    new("View Hangfire", FSHAction.View, FSHResource.Hangfire, IsAdmin: true),

    // Users
    new("View Users", FSHAction.View, FSHResource.Users, IsAdmin: true, IsManager: true),
    new("Search Users", FSHAction.Search, FSHResource.Users, IsAdmin: true, IsManager: true),
    new("Create Users", FSHAction.Create, FSHResource.Users, IsAdmin: true, IsManager: true),
    new("Update Users", FSHAction.Update, FSHResource.Users, IsAdmin: true, IsManager: true),
    new("Delete Users", FSHAction.Delete, FSHResource.Users, IsAdmin: true, IsManager: true),
    new("Export Users", FSHAction.Export, FSHResource.Users, IsAdmin: true, IsManager: true),

    // User Roles
    new("View UserRoles", FSHAction.View, FSHResource.UserRoles, IsAdmin: true),
    new("Update UserRoles", FSHAction.Update, FSHResource.UserRoles, IsAdmin: true, IsManager: true),

    // Roles
    new("View Roles", FSHAction.View, FSHResource.Roles, IsAdmin : true),
    new("Create Roles", FSHAction.Create, FSHResource.Roles, IsAdmin : true),
    new("Update Roles", FSHAction.Update, FSHResource.Roles, IsAdmin : true),
    new("Delete Roles", FSHAction.Delete, FSHResource.Roles, IsAdmin : true),

    // Role Claims
    new("View RoleClaims", FSHAction.View, FSHResource.RoleClaims, IsAdmin : true),
    new("Update RoleClaims", FSHAction.Update, FSHResource.RoleClaims, IsAdmin : true),

    // Products
    new("View Products", FSHAction.View, FSHResource.Products, IsAdmin: true, IsBasic: true),
    new("Search Products", FSHAction.Search, FSHResource.Products, IsAdmin: true, IsBasic: true),
    new("Create Products", FSHAction.Create, FSHResource.Products, IsAdmin : true),
    new("Update Products", FSHAction.Update, FSHResource.Products, IsAdmin : true),
    new("Delete Products", FSHAction.Delete, FSHResource.Products, IsAdmin : true),
    new("Export Products", FSHAction.Export, FSHResource.Products, IsAdmin : true),

    // Brands
    new("View Brands", FSHAction.View, FSHResource.Brands, IsAdmin : true, IsBasic: true),
    new("Search Brands", FSHAction.Search, FSHResource.Brands, IsAdmin : true, IsBasic: true),
    new("Create Brands", FSHAction.Create, FSHResource.Brands, IsAdmin : true),
    new("Update Brands", FSHAction.Update, FSHResource.Brands, IsAdmin : true),
    new("Delete Brands", FSHAction.Delete, FSHResource.Brands, IsAdmin : true),
    new("Generate Brands", FSHAction.Generate, FSHResource.Brands, IsAdmin : true),
    new("Clean Brands", FSHAction.Clean, FSHResource.Brands, IsAdmin : true),

    // Schools
    new("View Schools", FSHAction.View, FSHResource.Schools, IsAdmin: true, IsManager: true, IsContributor: true, IsBasic: true),
    new("Search Schools", FSHAction.Search, FSHResource.Schools, IsAdmin: true, IsManager: true, IsContributor: true, IsBasic: true),
    new("Export Schools", FSHAction.Export, FSHResource.Schools, IsAdmin: true, IsManager: true, IsContributor: true),
    new("Create Schools", FSHAction.Create, FSHResource.Schools, IsAdmin: true, IsManager: true),
    new("Update Schools", FSHAction.Update, FSHResource.Schools, IsAdmin: true, IsManager: true),
    new("Delete Schools", FSHAction.Delete, FSHResource.Schools, IsAdmin: true, IsManager: true),
  };

  public static IReadOnlyList<FSHPermission> All { get; } = new ReadOnlyCollection<FSHPermission>(_all);
  public static IReadOnlyList<FSHPermission> Admin { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsAdmin).ToArray());
  public static IReadOnlyList<FSHPermission> Manager { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsManager).ToArray());
  public static IReadOnlyList<FSHPermission> Contributor { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsContributor).ToArray());
  public static IReadOnlyList<FSHPermission> Basic { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record FSHPermission(string Description, string Action, string Resource, bool IsAdmin = false, bool IsManager = false, bool IsContributor = false, bool IsBasic = false)
{
  public string Name => NameFor(Action, Resource);
  public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
