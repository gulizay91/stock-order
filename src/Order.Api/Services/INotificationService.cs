using Order.Api.Persistence.Entities;

namespace Order.Api.Services;

public interface INotificationService
{
  Task PublishOrderNotificationEvent(OrderNotification orderNotification, int orderId, int clientId);
}