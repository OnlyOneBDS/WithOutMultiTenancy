using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WithOutMultiTenancy.Infrastructure.Middleware;

namespace WithOutMultiTenancy.Host.Controllers;

#nullable disable
#pragma warning disable RCS1163, IDE0060

public static class FSHApiConventions
{
  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Search([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Get()
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Get([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Get([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object cancellationtoken)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Post([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Post([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object cancellationToken)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Register([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Create([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Update([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Update([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Update([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object cancellationToken)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Put([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Put([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object cancellationToken)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Delete([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Delete([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object id, [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object cancellationToken)
  {
    // Method intentionally left empty.
  }

  [ProducesResponseType(200)]
  [ProducesResponseType(400, Type = typeof(HttpValidationProblemDetails))]
  [ProducesDefaultResponseType(typeof(ErrorResult))]
  [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
  public static void Generate([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)][ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)] object request)
  {
    // Method intentionally left empty.
  }
}