using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Order.Api.LifetimeHooks;
using Order.Api.Middlewares;
using Order.Api.Registrations;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Stock.Order.Api starting...");
ConfigureHostSettings(builder.Host);
Console.WriteLine("Configured Host Settings...");
ConfigurationSettings(builder.Configuration);
RegisterServices(builder.Services, builder.Configuration);
Console.WriteLine("Services Registered...");

var app = builder.Build();

ConfigureWebApplication(app);

app.Run();

void ConfigurationSettings(IConfigurationBuilder configurationBuilder)
{
  configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
  configurationBuilder.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
  configurationBuilder.AddEnvironmentVariables();
}

void ConfigureHostSettings(IHostBuilder hostBuilder)
{
  //https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0
  // Wait 30 seconds for graceful shutdown.
  hostBuilder.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));
}

void RegisterServices(IServiceCollection serviceCollection, IConfiguration configurationRoot)
{
  serviceCollection.AddHealthChecks();
  serviceCollection.RegisterLoggers(configurationRoot);
  serviceCollection.RegisterSwagger();
  serviceCollection.RegisterRepositories(configurationRoot);
  serviceCollection.RegisterMappers();
  serviceCollection.RegisterControllers();
  serviceCollection.RegisterMassTransit(configurationRoot);

  serviceCollection.AddHostedService<ApplicationLifetimeService>();
}

void ConfigureWebApplication(IApplicationBuilder applicationBuilder)
{
  var provider = applicationBuilder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
  applicationBuilder.UseHttpsRedirection();
  applicationBuilder
    .UseMiddleware<
      ExceptionHandlerMiddleware>();
  applicationBuilder.UseRouting();
  applicationBuilder.UseEndpoints(endpoints => { endpoints.MapControllers(); });
  applicationBuilder.RegisterHealthCheck();
  applicationBuilder.UseSwagger();
  applicationBuilder.UseSwaggerUI(options =>
  {
    // build a swagger endpoint for each discovered Order.Api version
    foreach (var description in provider.ApiVersionDescriptions)
      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
        description.GroupName.ToUpperInvariant());
  });
}