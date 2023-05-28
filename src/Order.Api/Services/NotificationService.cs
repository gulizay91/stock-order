using Contracts.Events.V1.Notification;
using MassTransit;
using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Enums;

namespace Order.Api.Services;

public class NotificationService : INotificationService
{
  private static readonly TimeSpan MessagePublishCancellationTokenTimeout = TimeSpan.FromSeconds(20);
  private readonly IBus _bus;
  private readonly ILogger<NotificationService> _logger;

  public NotificationService(IBus bus, ILogger<NotificationService> logger)
  {
    _logger = logger;
    _bus = bus;
  }

  public async Task PublishOrderNotificationEvent(OrderNotification orderNotification, int orderId)
  {
    var cancelationToken = new CancellationTokenSource(MessagePublishCancellationTokenTimeout);

    switch (orderNotification.NotificationType)
    {
      case NotificationType.Sms:
        await _bus.Publish(new OrderCreatedSmsEvent { OrderId = orderId, CorrelationId = Guid.NewGuid() },
          cancelationToken.Token);
        break;
      case NotificationType.Email:
        await _bus.Publish(new OrderCreatedEmailEvent { OrderId = orderId, CorrelationId = Guid.NewGuid() },
          cancelationToken.Token);
        break;
      case NotificationType.PushNotification:
        await _bus.Publish(new OrderCreatedPushNotificationEvent { OrderId = orderId, CorrelationId = Guid.NewGuid() },
          cancelationToken.Token);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(orderNotification.NotificationType),
          orderNotification.NotificationType, "UnKnown Notification channel!");
    }

    _logger.LogInformation(
      $"Published Message for Order: {orderNotification.Id}, channel: {orderNotification.NotificationType}");
  }
}