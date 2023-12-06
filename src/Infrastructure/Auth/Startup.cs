using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WithOutMultiTenancy.Application.Common.Interfaces;
using WithOutMultiTenancy.Infrastructure.Auth.Jwt;
using WithOutMultiTenancy.Infrastructure.Auth.Permissions;
using WithOutMultiTenancy.Infrastructure.Identity;

namespace WithOutMultiTenancy.Infrastructure.Auth;

internal static class Startup
{
  internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
  {
    return services
      .AddCurrentUser()
      .AddPermissions()
      // Must add identity before adding auth!
      .AddIdentity()
      .AddJwtAuth();

    //services
    //  .Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));   

    //return config["SecuritySettings:Provider"]!.Equals("AzureAd", StringComparison.OrdinalIgnoreCase)
    //  ? services.AddAzureAdAuth(config)
    //  : services.AddJwtAuth();
  }

  internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
  {
    return app
      .UseMiddleware<CurrentUserMiddleware>();
  }

  private static IServiceCollection AddCurrentUser(this IServiceCollection services)
  {
    return services
      .AddScoped<CurrentUserMiddleware>()
      .AddScoped<ICurrentUser, CurrentUser>()
      .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
  }

  private static IServiceCollection AddPermissions(this IServiceCollection services)
  {
    return services
      .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
      .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
  }
}