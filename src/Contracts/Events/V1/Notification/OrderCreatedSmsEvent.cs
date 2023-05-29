using Contracts.Events.V1.Notification.Abstract;

namespace Contracts.Events.V1.Notification;

public record OrderCreatedSmsEvent : IOrderNotificationEvent
{
  public int ClientId { get; init; }
  public int OrderId { get; init; }
  public Guid CorrelationId { get; set; }
}