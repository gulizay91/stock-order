using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Order.Api.Configurations;

public class ConfigureSwaggerOptions
  : IConfigureNamedOptions<SwaggerGenOptions>
{
  private readonly IApiVersionDescriptionProvider _provider;

  public ConfigureSwaggerOptions(
    IApiVersionDescriptionProvider provider)
  {
    _provider = provider;
  }

  public void Configure(SwaggerGenOptions options)
  {
    // add a swagger document for each discovered Order.Api version
    // note: you might choose to skip or document deprecated Order.Api versions differently
    foreach (var description in _provider.ApiVersionDescriptions)
      options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
  }

  public void Configure(string? name, SwaggerGenOptions options)
  {
    Configure(options);
  }

  private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
  {
    var info = new OpenApiInfo
    {
      Title = "Order.Api",
      Version = description.ApiVersion.ToString(),
      Description = "Stock.Order.Api",
      Contact = new OpenApiContact { Name = "Guliz AY", Email = "gulizay91@gmail.com" }
    };

    if (description.IsDeprecated) info.Description += " This Order.Api version has been deprecated.";

    return info;
  }
}