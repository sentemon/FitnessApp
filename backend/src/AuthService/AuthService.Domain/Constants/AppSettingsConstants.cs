namespace AuthService.Domain.Constants;

public static class AppSettingsConstants
{
    public const string WebHostUrl = "WebHostUrl";

    public const string AllowedOrigins = "AllowedOrigins";
    
    public const string DatabaseConnectionString = "DatabaseConnectionString";

    public const string Keycloak = "Keycloak";
    public const string KeycloakUrl = "Keycloak:Url";
    public const string KeycloakRealm = "Keycloak:Realm";
    public const string KeycloakClientId = "Keycloak:ClientId";
    public const string KeycloakClientSecret = "Keycloak:ClientSecret";
    public const string AdminUsername = "Keycloak:AdminUsername";
    public const string AdminPassword = "Keycloak:AdminPassword";
}