﻿using System.Security.Claims;
using Hangfire.Client;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WithOutMultiTenancy.Infrastructure.Common;

namespace WithOutMultiTenancy.Infrastructure.BackgroundJobs;

public class FSHJobFilter : IClientFilter
{
  private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
  private readonly IServiceProvider _services;

  public FSHJobFilter(IServiceProvider services)
  {
    _services = services;
  }

  public void OnCreating(CreatingContext context)
  {
    ArgumentNullException.ThrowIfNull(context, nameof(context));

    Logger.InfoFormat("Set TenantId and UserId parameters to job {0}.{1}...", context.Job.Method.ReflectedType?.FullName, context.Job.Method.Name);

    using var scope = _services.CreateScope();

    var httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext
      ?? throw new InvalidOperationException("Can't create a TenantJob without HttpContext.");

    string? userId = httpContext.User.GetUserId();
    context.SetJobParameter(QueryStringKeys.UserId, userId);
  }

  public void OnCreated(CreatedContext context)
  {
    Logger.InfoFormat(
      "Job created with parameters {0}",
      context.Parameters.Select(x => x.Key + "=" + x.Value).Aggregate((s1, s2) => s1 + ";" + s2));
  }
}