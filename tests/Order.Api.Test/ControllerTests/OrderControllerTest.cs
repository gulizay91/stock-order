using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Repositories;
using Order.Api.Services;
using Order.Api.Test.ObjectMothers;
using Order.Api.V1.Controllers;
using SystemTextJsonPatch;

namespace Order.Api.Test.ControllerTests;

public class OrderControllerTest
{
  private readonly Mock<ILogger<OrderController>> _mockLogger;
  private readonly Mock<INotificationService> _mockNotificationService;
  private readonly Mock<IOrderRepository> _mockOrderRepository;
  private readonly OrderController _sut;

  public OrderControllerTest()
  {
    _mockLogger = new Mock<ILogger<OrderController>>();
    _mockOrderRepository = new Mock<IOrderRepository>();
    _mockNotificationService = new Mock<INotificationService>();
    _sut = new OrderController(_mockLogger.Object, _mockOrderRepository.Object, _mockNotificationService.Object);
  }

  [Fact]
  public async Task QueryOrders_ShouldReturn200Status()
  {
    // Arrange
    var simpleOrder = OrderMother.SimpleOrders();
    var simpleQueryOrdersRequest = OrderMother.SimpleQueryOrdersRequest();
    _mockOrderRepository.Setup(_ => _.GetOrders(simpleQueryOrdersRequest.ClientId, null)).ReturnsAsync(simpleOrder);

    // Act
    var result = (OkObjectResult)await _sut.QueryOrders(simpleQueryOrdersRequest);

    // Assert
    result.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task CreateOrder_ShouldReturn201Status()
  {
    // Arrange
    var simpleRequest = OrderMother.SimpleCreateOrderRequest();
    var newOrder = OrderMother.SimpleOrder();
    _mockOrderRepository.Setup(_ => _.InsertOrder(It.IsAny<Persistence.Entities.Order>())).ReturnsAsync(newOrder);

    // Act
    var result = (CreatedResult)await _sut.CreateOrder(simpleRequest);

    // Assert
    result.StatusCode.Should().Be(201);
    _mockNotificationService.Verify(
      context => context.PublishOrderNotificationEvent(It.IsAny<OrderNotification>(), It.IsAny<int>(), It.IsAny<int>()),
      Times.Exactly(simpleRequest.OrderNotifications.Count()));
  }

  [Fact]
  public async Task CreateOrder_ShouldReturn409Status()
  {
    // Arrange
    var simpleOrder = OrderMother.SimpleOrder();
    var simpleRequest = OrderMother.CreateOrderRequestForClient(simpleOrder.ClientId);
    _mockOrderRepository.Setup(_ => _.InsertOrder(It.IsAny<Persistence.Entities.Order>())).Throws<DbUpdateException>();

    // Act
    var throwingAction = async () => { await _sut.CreateOrder(simpleRequest); };

    // Assert
    await Assert.ThrowsAsync<DbUpdateException>(throwingAction);
    _mockNotificationService.Verify(
      context => context.PublishOrderNotificationEvent(It.IsAny<OrderNotification>(), It.IsAny<int>(), It.IsAny<int>()),
      Times.Never);
  }

  [Fact]
  public async Task GetOrder_ShouldReturn200Status()
  {
    // Arrange
    var simpleOrder = OrderMother.SimpleOrder();
    _mockOrderRepository.Setup(_ => _.GetOrder(It.IsAny<int>())).ReturnsAsync(simpleOrder);

    // Act
    var result = (OkObjectResult)await _sut.GetOrder(simpleOrder.Id);

    // Assert
    result.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task GetOrder_ShouldReturn404Status()
  {
    // Arrange
    var simpleOrder = OrderMother.SimpleOrder();

    // Act
    var result = (NotFoundResult)await _sut.GetOrder(simpleOrder.Id);

    // Assert
    result.StatusCode.Should().Be(404);
  }

  [Fact]
  public async Task PatchOrder_ShouldReturn200Status()
  {
    // Arrange
    var simpleOrder = OrderMother.SimpleOrder();
    _mockOrderRepository
      .Setup(_ => _.UpdateOrderPatchAsync(It.IsAny<int>(), It.IsAny<JsonPatchDocument<Persistence.Entities.Order>>()))
      .ReturnsAsync(simpleOrder);

    // Act
    var result =
      (OkObjectResult)await _sut.PatchOrder(simpleOrder.Id, new JsonPatchDocument<Persistence.Entities.Order>());

    // Assert
    result.StatusCode.Should().Be(200);
  }

  [Fact]
  public async Task PatchOrder_ShouldReturn404Status()
  {
    // Arrange
    var simpleOrder = OrderMother.SimpleOrder();

    // Act
    var result =
      (NotFoundResult)await _sut.PatchOrder(simpleOrder.Id, new JsonPatchDocument<Persistence.Entities.Order>());

    // Assert
    result.StatusCode.Should().Be(404);
  }

  [Fact]
  public async Task QueryOrderNotifications_ShouldReturn200Status()
  {
    // Arrange
    var simpleOrderNotifications = OrderMother.SimpleOrderNotifications();
    _mockOrderRepository.Setup(_ => _.GetOrderNotifications(It.IsAny<int>())).ReturnsAsync(simpleOrderNotifications);

    // Act
    var result = (OkObjectResult)await _sut.QueryOrderNotifications(Random.Shared.Next());

    // Assert
    result.StatusCode.Should().Be(200);
  }
}