using MassTransit;
using Order.Api.Services;

namespace Order.Api.Registrations;

public static class MassTransitRegister
{
  public static void RegisterMassTransit(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    // ps: normally used configuration, validation and register these stuff, but now lets keep simple
    var clusterAddress = configuration
      .GetSection("BusSettings:ClusterAddress").Value;
    Console.Out.WriteLine($"BusSettings:ClusterAddress{clusterAddress}");
    var userName = configuration
      .GetSection("BusSettings:UserName").Value;
    Console.Out.WriteLine($"BusSettings:UserName{userName}");
    var password = configuration
      .GetSection("BusSettings:Password").Value;
    Console.Out.WriteLine($"BusSettings:Password{password}");

    serviceCollection.AddMassTransit(x =>
    {
      x.UsingRabbitMq((context, cfg) =>
      {
        cfg.Host(new Uri(clusterAddress), a =>
        {
          a.Username(userName);
          a.Password(password);
        });
      });
    });

    serviceCollection.AddScoped<INotificationService, NotificationService>();
  }
}