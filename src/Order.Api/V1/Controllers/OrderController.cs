using System.Net.Mime;
using System.Text.Json;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Middlewares.MiddlewareModels;
using Order.Api.Persistence.Repositories;
using Order.Api.Services;
using Order.Api.V1.Exchanges.Requests;
using Order.Api.V1.Exchanges.Responses;
using Swashbuckle.AspNetCore.Annotations;
using SystemTextJsonPatch;

namespace Order.Api.V1.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse))]
[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/orders")]
public class OrderController : ControllerBase
{
  private readonly ILogger<OrderController> _logger;
  private readonly INotificationService _notificatiponService;
  private readonly IOrderRepository _orderRepository;

  public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepository,
    INotificationService notificatiponService)
  {
    _logger = logger;
    _orderRepository = orderRepository;
    _notificatiponService = notificatiponService;
  }

  [HttpGet]
  [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ValidationResult))]
  [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<QueryOrdersResponse>))]
  public async Task<IActionResult> QueryOrders([FromQuery] QueryOrdersRequest request)
  {
    _logger.LogInformation(JsonSerializer.Serialize(request));
    var response = await _orderRepository.GetOrders(request.ClientId, request.Status);
    var result = response.Adapt<List<QueryOrdersResponse>>();
    return Ok(result);
  }

  [HttpPost]
  [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ValidationResult))]
  [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ErrorResponse))]
  [SwaggerResponse(StatusCodes.Status201Created, type: typeof(CreateOrderResponse))]
  public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
  {
    _logger.LogInformation(JsonSerializer.Serialize(request));
    var createOrder = request.Adapt<Persistence.Entities.Order>();
    var response = await _orderRepository.InsertOrder(createOrder);
    foreach (var orderNotification in response.OrderNotifications)
      await _notificatiponService.PublishOrderNotificationEvent(orderNotification, response.Id, createOrder.ClientId);
    return Created(new Uri(response.Id.ToString(), UriKind.Relative), response.Adapt<CreateOrderResponse>());
  }

  [HttpGet("{id:int}")]
  [SwaggerResponse(StatusCodes.Status404NotFound)]
  [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ValidationResult))]
  [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetOrderResponse))]
  public async Task<IActionResult> GetOrder([FromRoute] int id)
  {
    var response = await _orderRepository.GetOrder(id);
    var result = response?.Adapt<GetOrderResponse>();
    if (result is null) return NotFound();
    return Ok(result);
  }

  [HttpPatch("{id:int}")]
  [SwaggerResponse(StatusCodes.Status404NotFound)]
  [SwaggerResponse(StatusCodes.Status409Conflict, type: typeof(ErrorResponse))]
  [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PatchOrderResponse))]
  public async Task<IActionResult> PatchOrder([FromRoute] int id,
    [FromBody] JsonPatchDocument<Persistence.Entities.Order> request)
  {
    _logger.LogInformation(JsonSerializer.Serialize(request));
    var result = await _orderRepository.UpdateOrderPatchAsync(id, request);
    if (result is null) return NotFound();
    return Ok(result.Adapt<PatchOrderResponse>());
  }

  [HttpGet("{orderId:int}/notifications")]
  [SwaggerResponse(StatusCodes.Status400BadRequest, type: typeof(ValidationResult))]
  [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<QueryOrderNotificationsResponse>))]
  public async Task<IActionResult> QueryOrderNotifications([FromRoute] int orderId)
  {
    var updatedOrder = await _orderRepository.GetOrderNotifications(orderId);
    return Ok(updatedOrder.Adapt<List<QueryOrderNotificationsResponse>>());
  }
}