using Microsoft.AspNetCore.SignalR;
using WithOutMultiTenancy.Application.Common.Interfaces;
using WithOutMultiTenancy.Shared.Notifications;
using static WithOutMultiTenancy.Shared.Notifications.NotificationConstants;

namespace WithOutMultiTenancy.Infrastructure.Notifications;

public class NotificationSender : INotificationSender
{
  private readonly IHubContext<NotificationHub> _notificationHubContext;

  public NotificationSender(IHubContext<NotificationHub> notificationHubContext)
  {
    _notificationHubContext = notificationHubContext;
  }

  public Task BroadcastAsync(INotificationMessage notification, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .All
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task BroadcastAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .AllExcept(excludedConnectionIds)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .Group("TIRGroup")
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .GroupExcept("TIRGroup", excludedConnectionIds)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .Group(group)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToGroupAsync(INotificationMessage notification, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .GroupExcept(group, excludedConnectionIds)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .Groups(groupNames)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .User(userId)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }

  public Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken)
  {
    return _notificationHubContext.Clients
      .Users(userIds)
      .SendAsync(NotificationFromServer, notification.GetType().FullName, notification, cancellationToken);
  }
}