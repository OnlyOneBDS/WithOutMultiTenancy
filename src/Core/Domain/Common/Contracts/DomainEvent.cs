using WithOutMultiTenancy.Shared.Events;

namespace WithOutMultiTenancy.Domain.Common.Contracts;

public abstract class DomainEvent : IEvent
{
  public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}