using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Order.Api.Configurations;
using Order.Api.Filters;

namespace Order.Api.Registrations;

public static class SwaggerRegister
{
  public static void RegisterSwagger(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddEndpointsApiExplorer();

    serviceCollection.AddVersionedApiExplorer(setup =>
    {
      setup.GroupNameFormat = "'v'VVV";
      setup.SubstituteApiVersionInUrl = true;
    });

    serviceCollection.AddApiVersioning(options =>
    {
      options.Conventions.Add(new VersionByNamespaceConvention());
      options.DefaultApiVersion = new ApiVersion(1, 0);
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.ReportApiVersions = true;
    });

    serviceCollection.ConfigureOptions<ConfigureSwaggerOptions>();
    serviceCollection.AddSwaggerGen(options => options.SchemaFilter<PatchOrderSchemaFilter>());
  }
}