using System.Text.Json;
using Contracts.Events.V1.Notification;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Consumer.Clients;
using Notification.Consumer.Exceptions;
using Notification.Consumer.Persistence.Entities;
using Notification.Consumer.Persistence.Enum;
using Notification.Consumer.Persistence.Repositories;

namespace Notification.Consumer.Consumers;

public class SmsConsumer : IConsumer<OrderCreatedSmsEvent>
{
  private readonly ISmsProviderClient _httpClient;
  private readonly ILogger<SmsConsumer> _logger;
  private readonly IOrderNotificationEventRepository _orderNotificationEventRepository;

  public SmsConsumer(ILogger<SmsConsumer> logger, IOrderNotificationEventRepository orderNotificationEventRepository,
    ISmsProviderClient httpClient)
  {
    _logger = logger;
    _httpClient = httpClient;
    _orderNotificationEventRepository = orderNotificationEventRepository;
  }

  public async Task Consume(ConsumeContext<OrderCreatedSmsEvent> context)
  {
    _logger.LogInformation(JsonSerializer.Serialize(context.Message));
    var notificationEvent = new OrderNotificationEvent
    {
      Id = Guid.NewGuid(), OrderId = context.Message.OrderId, ClientId = context.Message.ClientId,
      NotificationType = NotificationType.Sms,
      Message =
        $"Client: {context.Message.ClientId}, Order: {context.Message.OrderId}, Event: {nameof(OrderCreatedSmsEvent)}",
      CreatedDate = DateTime.UtcNow
    };
    var providerResult = await _httpClient.SendMessageAsync(notificationEvent);
    if (!providerResult)
    {
      var message =
        $"ProviderClient {nameof(OrderCreatedSmsEvent)} event failed for OrderId {context.Message.OrderId}, CorrelationId: {context.Message.CorrelationId}";
      _logger.LogError(message);
      throw new NotificationEventException(message);
    }

    var result = await _orderNotificationEventRepository.InsertOrderNotificationEvent(notificationEvent);
    if (result == 0)
    {
      var message =
        $"{nameof(OrderCreatedSmsEvent)} event failed for OrderId {context.Message.OrderId}, CorrelationId: {context.Message.CorrelationId}";
      _logger.LogError(message);
      throw new NotificationEventException(message);
    }
  }
}