using Contracts.Events.V1.Notification;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Api.Services;
using Order.Api.Test.ObjectMothers;

namespace Order.Api.Test.ServiceTests;

public class NotificationServiceTest
{
  private readonly Mock<IBus> _mockBus;
  private readonly Mock<ILogger<NotificationService>> _mockLogger;
  private readonly NotificationService _sut;

  public NotificationServiceTest()
  {
    _mockLogger = new Mock<ILogger<NotificationService>>();
    _mockBus = new Mock<IBus>();
    _sut = new NotificationService(_mockBus.Object, _mockLogger.Object);
  }

  [Fact]
  public async Task CreateOrder_ShouldReturn201Status()
  {
    // Arrange
    var simpleOrderNotification = OrderMother.SimpleOrderNotificationSms();
    // Act
    await _sut.PublishOrderNotificationEvent(simpleOrderNotification, 1, 1);

    // Assert
    _mockBus.Verify(context => context.Publish(It.IsAny<OrderCreatedSmsEvent>(), It.IsAny<CancellationToken>()),
      Times.Once);
  }
}