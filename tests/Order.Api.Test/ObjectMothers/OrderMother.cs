using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Enums;
using Order.Api.V1.Exchanges.Common;
using Order.Api.V1.Exchanges.Requests;

namespace Order.Api.Test.ObjectMothers;

public static class OrderMother
{
  public static Persistence.Entities.Order SimpleOrder()
  {
    return new Persistence.Entities.Order
    {
      Id = Random.Shared.Next(1, 100),
      ClientId = Random.Shared.Next(1, 100),
      CryptoSymbol = CryptocurrencySymbol.BTC,
      DayOfMonth = 1,
      OrderType = OrderType.Buy,
      Price = 100,
      OrderNotifications = new List<OrderNotification>
      {
        new()
        {
          Id = Random.Shared.Next(1, 100),
          NotificationType = NotificationType.Sms
        }
      }
    };
  }

  public static List<Persistence.Entities.Order> SimpleOrders()
  {
    return new List<Persistence.Entities.Order>
    {
      new()
      {
        Id = Random.Shared.Next(1, 100),
        ClientId = Random.Shared.Next(1, 100),
        CryptoSymbol = CryptocurrencySymbol.BTC,
        DayOfMonth = 1,
        OrderType = OrderType.Buy,
        Price = 100,
        OrderNotifications = new List<OrderNotification>
        {
          new()
          {
            NotificationType = NotificationType.Sms
          }
        }
      }
    };
  }

  public static OrderNotification SimpleOrderNotificationSms()
  {
    return new OrderNotification
    {
      NotificationType = NotificationType.Sms
    };
  }

  public static List<OrderNotification> SimpleOrderNotifications()
  {
    return new List<OrderNotification>
    {
      new()
      {
        NotificationType = NotificationType.Sms
      }
    };
  }

  public static CreateOrderRequest SimpleCreateOrderRequest()
  {
    return new CreateOrderRequest
    {
      ClientId = Random.Shared.Next(1, 100),
      CryptoSymbol = CryptocurrencySymbol.BTC,
      DayOfMonth = 1,
      OrderType = OrderType.Buy,
      Price = 100,
      OrderNotifications = new List<OrderNotificationModel>
      {
        new()
        {
          NotificationType = NotificationType.Sms
        }
      }
    };
  }

  public static CreateOrderRequest SimpleBadCreateOrderRequest()
  {
    return new CreateOrderRequest
    {
      ClientId = Random.Shared.Next(1, 100),
      CryptoSymbol = CryptocurrencySymbol.BTC,
      DayOfMonth = 30,
      OrderType = OrderType.Buy,
      Price = Random.Shared.Next(1, 80),
      OrderNotifications = new List<OrderNotificationModel>
      {
        new()
        {
          NotificationType = NotificationType.Sms
        }
      }
    };
  }

  public static QueryOrdersRequest SimpleQueryOrdersRequest()
  {
    return new QueryOrdersRequest
    {
      ClientId = Random.Shared.Next(1, 100)
    };
  }
}