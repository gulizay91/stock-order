using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Order.Api.Persistence.Enums;
using Swashbuckle.AspNetCore.SwaggerGen;
using SystemTextJsonPatch;

namespace Order.Api.Filters;

public class PatchOrderSchemaFilter : ISchemaFilter
{
  public void Apply(OpenApiSchema schema, SchemaFilterContext context)
  {
    if (context.Type == typeof(JsonPatchDocument<Persistence.Entities.Order>))
      schema.Example = new OpenApiArray
      {
        new OpenApiObject
        {
          ["op"] = new OpenApiString("replace"),
          ["path"] = new OpenApiString("/status"),
          ["value"] = new OpenApiString(nameof(OrderStatus.Cancel))
        }
      };
  }
}