using Microsoft.EntityFrameworkCore;
using Order.Api.Extensions;
using Order.Api.Persistence;
using Order.Api.Persistence.Repositories;

namespace Order.Api.Registrations;

public static class RepositoryRegister
{
  public static void RegisterRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    var efCoreLogLevel = configuration.GetSection("Logging:LogLevel:Microsoft.EntityFrameworkCore").Value
      .ToEnum(LogLevel.Error);
    Console.Out.WriteLine($"Logging:LogLevel:Microsoft.EntityFrameworkCore: {efCoreLogLevel}");

    // ps: dataanotation or fluent api validations not working in memory db!
    // ref: https://github.com/dotnet/efcore/issues/7228
    // in-memory db
    var useInMemory = configuration.GetSection("ConnectionStrings")?.GetValue<bool>("UseInMemory") ?? false;
    Console.Out.WriteLine($"ConnectionStrings:UseInMemory: {useInMemory}");
    Console.Out.WriteLine($"ConnectionStrings:Default: {configuration.GetConnectionString("Default")}");
    if (useInMemory)
      serviceCollection.AddDbContextPool<OrderDatabaseContext>(options =>
        options.UseInMemoryDatabase("stock-order").EnableSensitiveDataLogging()
          .EnableThreadSafetyChecks().EnableDetailedErrors()
          .LogTo(Console.WriteLine, efCoreLogLevel));
    else
      // sqllocaldb
      serviceCollection.AddDbContextPool<OrderDatabaseContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Default")).EnableSensitiveDataLogging()
          .EnableThreadSafetyChecks().EnableDetailedErrors()
          .LogTo(Console.WriteLine, efCoreLogLevel));

    serviceCollection.AddTransient<IOrderRepository>(provider =>
      new OrderRepository(provider.GetRequiredService<OrderDatabaseContext>()));
  }
}