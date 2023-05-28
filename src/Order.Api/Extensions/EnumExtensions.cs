namespace Order.Api.Extensions;

public static class EnumExtensions
{
  public static T ToEnum<T>(this string? value, T defaultValue) where T : struct
  {
    if (string.IsNullOrWhiteSpace(value)) return defaultValue;

    T result;
    return Enum.TryParse(value, true, out result) ? result : defaultValue;
  }
}