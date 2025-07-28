namespace Shared.Authentication;

public class AuthenticationConfig
{
    public string Url { get; }
    public string Realm { get; }
    public string Audience { get; }
    public string Authority { get; }
    public string MetadataAddress { get; }
    public string ValidIssuer { get; }
    
    public AuthenticationConfig(string? url, string? realm, string? audience)
    {
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Realm = realm ?? throw new ArgumentNullException(nameof(realm));
        Audience = audience ?? throw new ArgumentNullException(nameof(audience));

        Authority = $"{url}/realms/{realm}";
        MetadataAddress = $"{url}/realms/fitness-app-realm/.well-known/openid-configuration";
        ValidIssuer = $"{url}/realms/{realm}";
    }
}