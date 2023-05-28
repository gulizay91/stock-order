using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Order.Api.Filters;

namespace Order.Api.Registrations;

public static class ControllerRegister
{
  public static void RegisterControllers(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddControllers().AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    serviceCollection.AddFluentValidationAutoValidation();
    serviceCollection.AddValidatorsFromAssembly(typeof(ValidationFilterAttribute<ValidationResult>).Assembly);
  }
}