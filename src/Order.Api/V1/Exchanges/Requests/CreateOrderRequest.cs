using Order.Api.Persistence.Enums;
using Order.Api.V1.Exchanges.Common;

namespace Order.Api.V1.Exchanges.Requests;

public record CreateOrderRequest
{
  public required int ClientId { get; init; }
  public required OrderType OrderType { get; init; }
  public required int DayOfMonth { get; init; }
  public required decimal Price { get; init; }
  public required CryptocurrencySymbol CryptoSymbol { get; init; }
  public required IEnumerable<OrderNotificationModel> OrderNotifications { get; init; }
}