using Order.Api.Extensions;

namespace Order.Api.Registrations;

public static class LoggerRegister
{
  public static void RegisterLoggers(this IServiceCollection serviceCollection, IConfiguration configuration)
  {
    var defaultLogLevel = configuration.GetSection("Logging:LogLevel:Default").Value.ToEnum(LogLevel.Error);
    Console.Out.WriteLine($"Console:LogLevel:Default: {defaultLogLevel}");
    serviceCollection.AddLogging(loggingBuilder => loggingBuilder
      .SetMinimumLevel(LogLevel.Debug));
  }
}