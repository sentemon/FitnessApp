namespace Shared.Authentication;

public class AuthenticationConfig
{
    public string Host { get; }
    public string Realm { get; }
    public string Audience { get; }
    public string Authority { get; }
    public string MetadataAddress { get; }
    public string ValidIssuer { get; }
    
    public AuthenticationConfig(string? host, string? realm, string? audience)
    {
        Host = host ?? throw new ArgumentNullException(nameof(host));
        Realm = realm ?? throw new ArgumentNullException(nameof(realm));
        Audience = audience ?? throw new ArgumentNullException(nameof(audience));

        Authority = $"{host}/realms/{realm}";
        MetadataAddress = $"{host}/realms/fitness-app-realm/.well-known/openid-configuration";
        ValidIssuer = $"{host}/realms/{realm}";
    }
}