using Order.Api.Persistence.Entities.Common;
using Order.Api.Persistence.Enums;

namespace Order.Api.Persistence.Entities;

public class Order : IEntity<int>
{
  public int ClientId { get; set; }
  public OrderType OrderType { get; set; }
  public int DayOfMonth { get; set; }
  public decimal Price { get; set; }
  public CryptocurrencySymbol CryptoSymbol { get; set; }
  public OrderStatus Status { get; set; }
  public AuditInformation AuditInformation { get; set; }

  public ICollection<OrderNotification> OrderNotifications { get; set; }
  public int Id { get; set; }
}