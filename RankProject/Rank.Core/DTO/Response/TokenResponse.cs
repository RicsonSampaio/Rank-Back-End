using System.Text.Json.Serialization;

namespace Rank.Core.DTO.Response;

public record TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expiration_date")]
    public DateTimeOffset ExpirationDate { get; set; }

    public TokenResponse(string accessToken, DateTimeOffset expirationDate)
    {
        AccessToken = accessToken;
        ExpirationDate = expirationDate;
    }
}
