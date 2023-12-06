using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithOutMultiTenancy.Infrastructure.Identity;
using WithOutMultiTenancy.Infrastructure.Persistence.Context;
using WithOutMultiTenancy.Shared.Authorization;

namespace WithOutMultiTenancy.Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
  private readonly RoleManager<ApplicationRole> _roleManager;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly CustomSeederRunner _seederRunner;
  private readonly ILogger<ApplicationDbSeeder> _logger;

  public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
  {
    _roleManager = roleManager;
    _userManager = userManager;
    _seederRunner = seederRunner;
    _logger = logger;
  }

  public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
  {
    await SeedRolesAsync(dbContext);
    await SeedAdminUserAsync();
    await _seederRunner.RunSeedersAsync(cancellationToken);
  }

  private async Task SeedRolesAsync(ApplicationDbContext dbContext)
  {
    foreach (string roleName in FSHRoles.DefaultRoles)
    {
      if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName) is not ApplicationRole role)
      {
        // Create the role
        _logger.LogInformation("Seeding {role} role...", roleName);

        role = roleName switch
        {
          "Admin" => new ApplicationRole(roleName, "Has full access"),
          "Manager" => new ApplicationRole(roleName, "Has some admin access"),
          "Contributor" => new ApplicationRole(roleName, "Has access to their school only"),
          _ => new ApplicationRole(roleName, "Has read only access to certain parts of the app"),
        };

        await _roleManager.CreateAsync(role);
      }

      // Assign permissions
      switch (roleName)
      {
        case FSHRoles.Admin:
          await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Admin, role);
          break;
        case FSHRoles.Manager:
          await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Manager, role);
          break;
        case FSHRoles.Contributor:
          await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Contributor, role);
          break;
        default:
          await AssignPermissionsToRoleAsync(dbContext, FSHPermissions.Basic, role);
          break;
      }
    }
  }

  private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<FSHPermission> permissions, ApplicationRole role)
  {
    var currentClaims = await _roleManager.GetClaimsAsync(role);

    foreach (var permission in permissions)
    {
      if (!currentClaims.Any(c => c.Type == FSHClaims.Permission && c.Value == permission.Name))
      {
        _logger.LogInformation("Seeding {role} permission '{permission}'...", role.Name, permission.Name);

        dbContext.RoleClaims.Add(new ApplicationRoleClaim
        {
          RoleId = role.Id,
          ClaimType = FSHClaims.Permission,
          ClaimValue = permission.Name,
          CreatedBy = "ApplicationDbSeeder",
          CreatedOn = DateTime.Now
        });

        await dbContext.SaveChangesAsync();
      }
    }
  }

  private async Task SeedAdminUserAsync()
  {
    if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == "edwidge.amisial.c@palmbeachschools.org") is not ApplicationUser adminUser)
    {
      adminUser = new ApplicationUser
      {
        FirstName = "Edwidge",
        LastName = "Amisial",
        Email = "edwidge.amisial.c@palmbeachschools.org",
        UserName = "c-amisiale",
        PhoneNumber = "5612474049",
        NormalizedEmail = "EDWIDGE.AMISIAL.C@PALMBEACHSCHOOLS.ORG",
        NormalizedUserName = "C-AMISIALE",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
        IsActive = true
      };

      _logger.LogInformation("Seeding default admin user...");

      var password = new PasswordHasher<ApplicationUser>();

      adminUser.PasswordHash = password.HashPassword(adminUser, "123Pa$$word!");
      await _userManager.CreateAsync(adminUser);
    }

    // Assign role to user
    if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.Admin))
    {
      _logger.LogInformation("Assigning admin role to admin user...");
      await _userManager.AddToRoleAsync(adminUser, FSHRoles.Admin);
    }
  }
}