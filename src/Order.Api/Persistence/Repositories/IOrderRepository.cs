using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Enums;
using SystemTextJsonPatch;

namespace Order.Api.Persistence.Repositories;

public interface IOrderRepository
{
  Task<Entities.Order?> GetOrder(int id);
  Task<List<Entities.Order>> GetOrders(int clientId, OrderStatus? status = null);
  Task<Entities.Order> InsertOrder(Entities.Order order);
  Task<Entities.Order?> UpdateOrderPatchAsync(int orderId, JsonPatchDocument<Entities.Order> orderDocument);
  Task<List<OrderNotification>> GetOrderNotifications(int orderId);
}