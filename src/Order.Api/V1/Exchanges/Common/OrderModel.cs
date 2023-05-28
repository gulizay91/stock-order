using Order.Api.Persistence.Enums;

namespace Order.Api.V1.Exchanges.Common;

public record OrderModel
{
  public int Id { get; init; }
  public int ClientId { get; init; }
  public OrderType OrderType { get; init; }
  public int DayOfMonth { get; init; }
  public decimal Price { get; init; }
  public CryptocurrencySymbol CryptoSymbol { get; init; }
  public OrderStatus Status { get; init; }
}