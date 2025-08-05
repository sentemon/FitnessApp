using System.Text.Json.Serialization;

namespace AuthService.Infrastructure.Models;

public class KeycloakErrorResponse
{
    [JsonPropertyName("error")]
    public required string Error { get; set; }

    [JsonPropertyName("error_description")]
    public required string ErrorDescription { get; set; }
}
