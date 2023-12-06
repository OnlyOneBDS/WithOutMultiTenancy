using WithOutMultiTenancy.Shared.Events;

namespace WithOutMultiTenancy.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
  Task PublishAsync(IEvent @event);
}