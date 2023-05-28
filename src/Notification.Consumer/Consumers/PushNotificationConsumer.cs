using System.Text.Json;
using Contracts.Events.V1.Notification;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.Consumer.Consumers;

public class PushNotificationConsumer : IConsumer<OrderCreatedPushNotificationEvent>
{
  private readonly ILogger<PushNotificationConsumer> _logger;

  public PushNotificationConsumer(ILogger<PushNotificationConsumer> logger)
  {
    _logger = logger;
  }
  
  public Task Consume(ConsumeContext<OrderCreatedPushNotificationEvent> context)
  {
    _logger.LogInformation(JsonSerializer.Serialize(context.Message));
    return Task.CompletedTask;
  }
}