using System.Reflection;
using System.Runtime.CompilerServices;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WithOutMultiTenancy.Infrastructure.Auth;
using WithOutMultiTenancy.Infrastructure.BackgroundJobs;
using WithOutMultiTenancy.Infrastructure.Caching;
using WithOutMultiTenancy.Infrastructure.Common;
using WithOutMultiTenancy.Infrastructure.Cors;
using WithOutMultiTenancy.Infrastructure.FileStorage;
using WithOutMultiTenancy.Infrastructure.Localization;
using WithOutMultiTenancy.Infrastructure.Mailing;
using WithOutMultiTenancy.Infrastructure.Mapping;
using WithOutMultiTenancy.Infrastructure.Middleware;
using WithOutMultiTenancy.Infrastructure.Notifications;
using WithOutMultiTenancy.Infrastructure.OpenApi;
using WithOutMultiTenancy.Infrastructure.Persistence;
using WithOutMultiTenancy.Infrastructure.Persistence.Initialization;
using WithOutMultiTenancy.Infrastructure.SecurityHeaders;
using WithOutMultiTenancy.Infrastructure.Validations;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace WithOutMultiTenancy.Infrastructure;

public static class Startup
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
  {
    var applicationAssembly = typeof(Application.Startup).GetTypeInfo().Assembly;
    MapsterSettings.Configure();

    return services
      .AddApiVersioning()
      .AddAuth(config)
      .AddBackgroundJobs(config)
      .AddCaching(config)
      .AddCorsPolicy(config)
      .AddExceptionMiddleware()
      .AddBehaviours(applicationAssembly)
      .AddHealthCheck()
      .AddPOLocalization(config)
      .AddMailing(config)
      .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
      .AddNotifications(config)
      .AddOpenApiDocumentation(config)
      .AddPersistence()
      .AddRequestLogging(config)
      .AddRouting(options => options.LowercaseUrls = true)
      .AddServices();
  }

  private static IServiceCollection AddApiVersioning(this IServiceCollection services)
  {
    services.AddApiVersioning(config =>
    {
      config.DefaultApiVersion = new ApiVersion(1, 0);
      config.AssumeDefaultVersionWhenUnspecified = true;
      config.ReportApiVersions = true;
    });

    return services;
  }

  private static IServiceCollection AddHealthCheck(this IServiceCollection services)
  {
    return services.AddHealthChecks().Services;
  }

  public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
  {
    // Create a new scope to retrieve scoped services
    using var scope = services.CreateScope();

    await scope
      .ServiceProvider
      .GetRequiredService<IDatabaseInitializer>()
      .InitializeDatabasesAsync(cancellationToken);
  }

  public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
  {
    builder
      .UseRequestLocalization()
      .UseStaticFiles()
      .UseSecurityHeaders(config)
      .UseFileStorage()
      .UseExceptionMiddleware()
      .UseRouting()
      .UseCorsPolicy()
      .UseAuthentication()
      .UseCurrentUser()
      .UseAuthorization()
      .UseRequestLogging(config)
      .UseHangfireDashboard(config)
      .UseOpenApiDocumentation(config);

    return builder;
  }

  public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
  {
    builder.MapControllers().RequireAuthorization();
    builder.MapHealthCheck();
    builder.MapNotifications();

    return builder;
  }

  private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints)
  {
    return endpoints.MapHealthChecks("/api/health");
  }
}