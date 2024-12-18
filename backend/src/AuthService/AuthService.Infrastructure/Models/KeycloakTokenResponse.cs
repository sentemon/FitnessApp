using System.Text.Json.Serialization;

namespace AuthService.Infrastructure.Models;

public class KeycloakTokenResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    
    [JsonPropertyName("expires_in")]
    public required long ExpiresIn { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
    
    public required long RefreshExpiresIn { get; set; }
    
    [JsonPropertyName("token_type")]
    public required string TokenType { get; set; }
    
    [JsonPropertyName("not-before-policy")]
    public required long NotBeforePolicy { get; set; }
    
    [JsonPropertyName("session_state")]
    public required Guid SessionState { get; set; }
    
    [JsonPropertyName("scope")]
    public required string Scope { get; set; }
}