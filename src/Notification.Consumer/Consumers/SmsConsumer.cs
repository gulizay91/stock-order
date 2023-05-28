using System.Text.Json;
using Contracts.Events.V1.Notification;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.Consumer.Consumers;

public class SmsConsumer : IConsumer<OrderCreatedSmsEvent>
{
  private readonly ILogger<SmsConsumer> _logger;

  public SmsConsumer(ILogger<SmsConsumer> logger)
  {
    _logger = logger;
  }

  public Task Consume(ConsumeContext<OrderCreatedSmsEvent> context)
  {
    _logger.LogInformation(JsonSerializer.Serialize(context.Message));
    return Task.CompletedTask;
  }
}