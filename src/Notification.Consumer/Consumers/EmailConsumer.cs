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

public class EmailConsumer : IConsumer<OrderCreatedEmailEvent>
{
  private readonly IEmailProviderClient _httpClient;
  private readonly ILogger<EmailConsumer> _logger;
  private readonly IOrderNotificationEventRepository _orderNotificationEventRepository;

  public EmailConsumer(ILogger<EmailConsumer> logger,
    IOrderNotificationEventRepository orderNotificationEventRepository, IEmailProviderClient httpClient)
  {
    _logger = logger;
    _httpClient = httpClient;
    _orderNotificationEventRepository = orderNotificationEventRepository;
  }

  public async Task Consume(ConsumeContext<OrderCreatedEmailEvent> context)
  {
    _logger.LogInformation(JsonSerializer.Serialize(context.Message));
    var notificationEvent = new OrderNotificationEvent
    {
      Id = Guid.NewGuid(), OrderId = context.Message.OrderId, ClientId = context.Message.ClientId,
      NotificationType = NotificationType.Email,
      Message =
        $"Client: {context.Message.ClientId}, Order: {context.Message.OrderId}, Event: {nameof(OrderCreatedEmailEvent)}",
      CreatedDate = DateTime.UtcNow
    };
    var providerResult = await _httpClient.SendMessageAsync(notificationEvent);
    if (!providerResult)
    {
      var message =
        $"ProviderClient {nameof(OrderCreatedEmailEvent)} event failed for OrderId {context.Message.OrderId}, CorrelationId: {context.Message.CorrelationId}";
      _logger.LogError(message);
      throw new NotificationEventException(message);
    }

    var result = await _orderNotificationEventRepository.InsertOrderNotificationEvent(notificationEvent);
    if (result == 0)
    {
      var message =
        $"{nameof(OrderCreatedEmailEvent)} event failed for OrderId {context.Message.OrderId}, CorrelationId: {context.Message.CorrelationId}";
      _logger.LogError(message);
      throw new NotificationEventException(message);
    }
  }
}