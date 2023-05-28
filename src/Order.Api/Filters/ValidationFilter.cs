using FluentValidation;

namespace Order.Api.Filters;

public class ValidationFilterAttribute<T> : IEndpointFilter where T : class
{
  private readonly IValidator<T> _validators;

  public ValidationFilterAttribute(IValidator<T> validators)
  {
    _validators = validators;
  }

  public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
  {
    var request = context.Arguments.FirstOrDefault(r => r!.GetType() == typeof(T)) as T;

    var result = await _validators.ValidateAsync(request!);
    if (!result.IsValid) return Results.Json(result.Errors, statusCode: StatusCodes.Status400BadRequest);

    return await next(context);
  }
}