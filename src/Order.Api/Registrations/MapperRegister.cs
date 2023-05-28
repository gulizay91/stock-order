using Mapster;
using Order.Api.Persistence.Entities;
using Order.Api.Persistence.Entities.Common;
using Order.Api.V1.Exchanges.Common;
using Order.Api.V1.Exchanges.Requests;

namespace Order.Api.Registrations;

public static class MapperRegister
{
  public static void RegisterMappers(this IServiceCollection serviceCollection)
  {
    TypeAdapterConfig<CreateOrderRequest, Persistence.Entities.Order>.NewConfig()
      .Map(dest => dest.AuditInformation,
        src => new AuditInformation { CreatedDate = DateTime.UtcNow });

    TypeAdapterConfig<OrderNotificationModel, OrderNotification>.NewConfig();
    TypeAdapterConfig<PatchOrderRequest, Persistence.Entities.Order>.NewConfig();
  }
}