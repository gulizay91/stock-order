using Contracts.Common;

namespace Contracts.Events.V1.Notification.Abstract;

public interface IOrderNotificationEvent : IEvent
{
  public int OrderId { get; init; }
  public Guid CorrelationId { get; set; }
}