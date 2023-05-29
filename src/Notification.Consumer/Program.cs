// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Consumer.Registrations;

var hostBuilder = new HostBuilder()
  .ConfigureHostConfiguration(configHost =>
    configHost.AddEnvironmentVariables("ASPNETCORE_")
  )
#if DEBUG
  .UseEnvironment("Development")
#endif
  .ConfigureAppConfiguration((hostingContext, config) =>
  {
    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true);
    config.AddEnvironmentVariables();
    Console.WriteLine($"{hostingContext.HostingEnvironment.EnvironmentName}");
  })
  .ConfigureServices(ConfigureServices)
  .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
  {
    loggingBuilder.AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"));
    loggingBuilder.AddConsole();
  });

await hostBuilder.RunConsoleAsync();

void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
  services.RegisterLoggers(hostContext.Configuration);
  services.RegisterMassTransit(hostContext.Configuration);
  services.RegisterRepositories(hostContext.Configuration);
  services.RegisterHttpClients();

  services.Configure<HostOptions>(opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(45));
}