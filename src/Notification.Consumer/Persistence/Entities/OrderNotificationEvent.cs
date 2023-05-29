using Notification.Consumer.Persistence.Entities.Common;
using Notification.Consumer.Persistence.Enum;

namespace Notification.Consumer.Persistence.Entities;

public class OrderNotificationEvent : IEntity<Guid>
{
  public int OrderId { get; set; }
  public int ClientId { get; set; }
  public NotificationType NotificationType { get; set; }
  public string Message { get; set; }
  public DateTime CreatedDate { get; init; }
  public Guid Id { get; set; }
}