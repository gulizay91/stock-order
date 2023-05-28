using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Api.Persistence.Repositories;
using Order.Api.Test.ObjectMothers;
using Order.Api.V1.Controllers;
using SystemTextJsonPatch;

namespace Order.Api.Test.ControllerTests;

public class OrderControllerTest
{
  private readonly Mock<ILogger<OrderController>> _logger;
  private readonly Mock<IOrderRepository> _mockOrderRepository;
  private readonly OrderController _sut;


  public OrderControllerTest()
  {
    _logger = new Mock<ILogger<OrderController>>();
    _mockOrderRepository = new Mock<IOrderRepository>();
    _sut = new OrderController(_logger.Object, _mockOrderRepository.Object);
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