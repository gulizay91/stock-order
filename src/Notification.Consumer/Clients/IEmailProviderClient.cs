using Notification.Consumer.Persistence.Entities;

namespace Notification.Consumer.Clients;

public interface IEmailProviderClient
{
  public async Task<bool> SendMessageAsync(OrderNotificationEvent notificationEvent)
  {
    return await Task.FromResult(true);
  }
}