using System.Text.Json.Serialization;

namespace Order.Api.V1.Exchanges.Common.Abstracts;

public abstract record BaseQueryRequest
{
  [JsonPropertyName("page")] public int? Page { get; init; }
  [JsonPropertyName("limit")] public int? Limit { get; init; }
}