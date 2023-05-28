using FluentValidation;
using Order.Api.V1.Exchanges.Requests;

namespace Order.Api.V1.Exchanges.Validations;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
  public CreateOrderValidator()
  {
    RuleFor(t => t.ClientId).NotNull();
    RuleFor(t => t.Price).InclusiveBetween(100, 20_000);
    RuleFor(x => x.DayOfMonth).InclusiveBetween(1, 28);
  }
}