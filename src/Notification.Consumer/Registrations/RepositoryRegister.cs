using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Consumer.Persistence;
using Notification.Consumer.Persistence.Repositories;

namespace Notification.Consumer.Registrations;

public static class RepositoryRegister
{
  public static void RegisterRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    // ps: dataanotation or fluent api validations not working in memory db!
    // ref: https://github.com/dotnet/efcore/issues/7228
    // in-memory db
    var useInMemory = configuration.GetSection("ConnectionStrings")?.GetValue<bool>("UseInMemory") ?? false;
    Console.Out.WriteLine($"ConnectionStrings:UseInMemory: {useInMemory}");
    Console.Out.WriteLine($"ConnectionStrings:Default: {configuration.GetConnectionString("Default")}");
    if (useInMemory)
      serviceCollection.AddDbContextPool<NotificationDatabaseContext>(options =>
        options.UseInMemoryDatabase("stock-notification"));
    else
      // sqllocaldb
      serviceCollection.AddDbContextPool<NotificationDatabaseContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Default")));

    serviceCollection.AddTransient<IOrderNotificationEventRepository>(provider =>
      new OrderNotificationEventRepository(provider.GetRequiredService<NotificationDatabaseContext>()));
  }
}