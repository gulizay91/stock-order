using System.Text.Json;
using Contracts.Events.V1.Notification;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Notification.Consumer.Consumers;

public class EmailConsumer : IConsumer<OrderCreatedEmailEvent>
{
  private readonly ILogger<EmailConsumer> _logger;

  public EmailConsumer(ILogger<EmailConsumer> logger)
  {
    _logger = logger;
  }
  
  public Task Consume(ConsumeContext<OrderCreatedEmailEvent> context)
  {
    _logger.LogInformation(JsonSerializer.Serialize(context.Message));
    return Task.CompletedTask;
  }
}