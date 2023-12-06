namespace WithOutMultiTenancy.Application.Common.Caching;

public static class CacheKeyServiceExtensions
{
  public static string GetCacheKey<TEntity>(this ICacheKeyService cacheKeyService, object id) where TEntity : IEntity
  {
    return cacheKeyService.GetCacheKey(typeof(TEntity).Name, id);
  }
}