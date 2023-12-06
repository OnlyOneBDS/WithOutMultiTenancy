using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace WithOutMultiTenancy.Infrastructure.FileStorage;

internal static class Startup
{
  internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
  {
    return app.UseStaticFiles(new StaticFileOptions()
    {
      FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
      RequestPath = new PathString("/Files")
    });
  }
}