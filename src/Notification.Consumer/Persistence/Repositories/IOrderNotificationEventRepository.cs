using Notification.Consumer.Persistence.Entities;

namespace Notification.Consumer.Persistence.Repositories;

public interface IOrderNotificationEventRepository
{
  Task<int> InsertOrderNotificationEvent(OrderNotificationEvent notificationEvent);
  Task<List<OrderNotificationEvent>> GetOrderNotificationEvents(int orderId);
}