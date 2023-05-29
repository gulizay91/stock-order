using Microsoft.EntityFrameworkCore;
using Notification.Consumer.Persistence.Entities;

namespace Notification.Consumer.Persistence.Repositories;

public class OrderNotificationEventRepository : IOrderNotificationEventRepository
{
  private readonly NotificationDatabaseContext _notificationDatabaseContext;

  public OrderNotificationEventRepository(NotificationDatabaseContext orderDatabaseContext)
  {
    _notificationDatabaseContext = orderDatabaseContext;
  }

  public async Task<int> InsertOrderNotificationEvent(OrderNotificationEvent notificationEvent)
  {
    _notificationDatabaseContext.Add(notificationEvent);
    return await _notificationDatabaseContext.SaveChangesAsync();
  }

  public async Task<List<OrderNotificationEvent>> GetOrderNotificationEvents(int orderId)
  {
    return await _notificationDatabaseContext.OrderNotificationEvent.Where(r => r.OrderId == orderId).ToListAsync();
  }
}