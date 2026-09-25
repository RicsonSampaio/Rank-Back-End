using System.Text.Json.Serialization;

namespace Rank.Core.DTO.Response;

public sealed record TokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("expiration_date")] DateTimeOffset ExpirationDate);
