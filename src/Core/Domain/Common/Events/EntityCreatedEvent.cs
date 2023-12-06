namespace WithOutMultiTenancy.Domain.Common.Events;

public static class EntityCreatedEvent
{
  public static EntityCreatedEvent<TEntity> WithEntity<TEntity>(TEntity entity) where TEntity : IEntity
  {
    return new(entity);
  }
}

public class EntityCreatedEvent<TEntity> : DomainEvent where TEntity : IEntity
{
  public TEntity Entity { get; }

  internal EntityCreatedEvent(TEntity entity)
  {
    Entity = entity;
  }
}