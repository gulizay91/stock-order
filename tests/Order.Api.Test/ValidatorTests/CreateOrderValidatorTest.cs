using FluentValidation.TestHelper;
using Order.Api.Test.ObjectMothers;
using Order.Api.V1.Exchanges.Validations;

namespace Order.Api.Test.ValidatorTests;

public class CreateOrderValidatorTest
{
  private readonly CreateOrderValidator _sut;

  public CreateOrderValidatorTest()
  {
    _sut = new CreateOrderValidator();
  }

  [Fact]
  public async Task CreateOrderRequest_ShouldHaveError_Price_DayOfMonth()
  {
    // Arrange
    var simpleBadRequest = OrderMother.SimpleBadCreateOrderRequest();
    // Act
    var result = await _sut.TestValidateAsync(simpleBadRequest);

    // Assert
    result.ShouldHaveValidationErrorFor(r => r.Price);
    result.ShouldHaveValidationErrorFor(r => r.DayOfMonth);
  }
}