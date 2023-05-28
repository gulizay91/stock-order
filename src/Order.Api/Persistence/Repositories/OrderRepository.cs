using Microsoft.EntityFrameworkCore;
using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Enums;
using SystemTextJsonPatch;

namespace Order.Api.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
  private readonly OrderDatabaseContext _orderDatabaseContext;

  public OrderRepository(OrderDatabaseContext orderDatabaseContext)
  {
    _orderDatabaseContext = orderDatabaseContext;
  }

  public async Task<Entities.Order?> GetOrder(int id)
  {
    return await _orderDatabaseContext.Order.FindAsync(id);
  }

  public async Task<List<Entities.Order>> GetOrders(int clientId, OrderStatus? status)
  {
    var query = _orderDatabaseContext.Order.AsNoTracking().Where(r => r.ClientId == clientId);
    if (status.HasValue)
      query = query.Where(r => r.Status == status.Value);

    return await query.ToListAsync();
  }

  public async Task<Entities.Order> InsertOrder(Entities.Order order)
  {
    _orderDatabaseContext.Add(order);
    await _orderDatabaseContext.SaveChangesAsync();
    return order;
  }

  public async Task<Entities.Order?> UpdateOrderPatchAsync(int orderId, JsonPatchDocument<Entities.Order> orderDocument)
  {
    var order = await GetOrder(orderId);
    if (order == null) return order;

    // ref: https://github.com/dotnet/aspnetcore/issues/24333
    // https://github.com/Havunen/SystemTextJsonPatch/tree/main
    order.AuditInformation = order.AuditInformation with { LastModifiedDate = DateTime.UtcNow };
    orderDocument.ApplyTo(order);
    await _orderDatabaseContext.SaveChangesAsync();

    return order;
  }

  public async Task<List<OrderNotification>> GetOrderNotifications(int orderId)
  {
    var query = _orderDatabaseContext.Order.Include(r => r.OrderNotifications)
      .Where(r => r.Id == orderId).SelectMany(r => r.OrderNotifications);

    return await query.AsNoTracking().ToListAsync();
  }
}