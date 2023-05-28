using Order.Api.Persistence.Enums;

namespace Order.Api.V1.Exchanges.Requests;

public record QueryOrdersRequest
{
  public required int ClientId { get; init; }
  public OrderStatus? Status { get; init; }
}