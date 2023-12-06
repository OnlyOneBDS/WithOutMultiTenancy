using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace WithOutMultiTenancy.Application;

public static class Startup
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    var assembly = Assembly.GetExecutingAssembly();

    return services
      .AddValidatorsFromAssembly(assembly)
      .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
  }
}