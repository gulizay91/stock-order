using Order.Api.Persistence.Enums;

namespace Order.Api.V1.Exchanges.Common;

public record OrderNotificationModel
{
  public required NotificationType NotificationType { get; init; }
}