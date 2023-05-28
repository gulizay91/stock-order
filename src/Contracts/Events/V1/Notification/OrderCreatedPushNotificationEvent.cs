using Contracts.Events.V1.Notification.Abstract;

namespace Contracts.Events.V1.Notification;

public record OrderCreatedPushNotificationEvent : IOrderNotificationEvent
{
  public int OrderId { get; init; }
  public Guid CorrelationId { get; set; }
}