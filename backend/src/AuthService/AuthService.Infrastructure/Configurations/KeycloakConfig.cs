namespace AuthService.Infrastructure.Configurations;

public class KeycloakConfig
{
    public string Url { get; }
    public string Realm { get; }
    public string ClientId { get; }
    public string ClientSecret { get; }
    public string AdminUsername { get; }
    public string AdminPassword { get; }
    
    public KeycloakConfig(string url, string realm, string clientId, string clientSecret, string adminUsername, string adminPassword)
    {
        Url = url ?? throw new ArgumentNullException(nameof(url), "Keycloak URL is not configured.");
        Realm = realm ?? throw new ArgumentNullException(nameof(realm), "Keycloak Realm is not configured.");
        ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId), "Keycloak ClientId is not configured.");
        ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret), "Keycloak Client Secret is not configured.");
        AdminUsername = adminUsername ?? throw new ArgumentNullException(nameof(adminUsername), "Keycloak Admin Username is not configured.");
        AdminPassword = adminPassword ?? throw new ArgumentNullException(nameof(adminPassword), "Keycloak Admin Password is not configured.");
    }
}
