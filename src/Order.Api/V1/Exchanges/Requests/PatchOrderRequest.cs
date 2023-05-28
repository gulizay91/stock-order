using Order.Api.Persistence.Enums;

namespace Order.Api.V1.Exchanges.Requests;

public record PatchOrderRequest
{
  public OrderStatus Status { get; init; }
}