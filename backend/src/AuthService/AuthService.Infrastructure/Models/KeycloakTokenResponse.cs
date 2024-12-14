using System.Text.Json.Serialization;

namespace AuthService.Infrastructure.Models;

public sealed class KeycloakTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    
    public long RefreshExpiresIn { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    
    [JsonPropertyName("not-before-policy")]
    public long NotBeforePolicy { get; set; }
    
    [JsonPropertyName("session_state")]
    public Guid SessionState { get; set; }
    
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}