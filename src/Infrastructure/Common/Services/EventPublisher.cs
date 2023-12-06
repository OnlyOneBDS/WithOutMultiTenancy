using MediatR;
using Microsoft.Extensions.Logging;
using WithOutMultiTenancy.Application.Common.Events;
using WithOutMultiTenancy.Shared.Events;

namespace WithOutMultiTenancy.Infrastructure.Common.Services;

public class EventPublisher : IEventPublisher
{
  private readonly ILogger<EventPublisher> _logger;
  private readonly IPublisher _mediator;

  public EventPublisher(ILogger<EventPublisher> logger, IPublisher mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public Task PublishAsync(IEvent @event)
  {
    _logger.LogInformation("Publishing Event : {event}", @event.GetType().Name);
    return _mediator.Publish(CreateEventNotification(@event));
  }

  private static INotification CreateEventNotification(IEvent @event)
  {
    return (INotification)Activator.CreateInstance(typeof(EventNotification<>).MakeGenericType(@event.GetType()), @event)!;
  }
}