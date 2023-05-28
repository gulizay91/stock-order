using Order.Api.Persistence.Entities.Common;
using Order.Api.Persistence.Enums;

namespace Order.Api.Persistence.Entities;

public class OrderNotification : IEntity<int>
{
  public NotificationType NotificationType { get; set; }
  public int Id { get; set; }
}