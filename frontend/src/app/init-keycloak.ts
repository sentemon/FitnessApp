import { KeycloakService } from 'keycloak-angular';
import { environment } from "../environments/environment";

const Keycloak = typeof window !== 'undefined' ? import('keycloak-js') : null;

export function initKeycloak(keycloak: KeycloakService) {
  if (Keycloak !== null) {
    return () =>
      keycloak.init({
        config: {
          url: environment.keycloak.url,
          realm: environment.keycloak.realm,
          clientId: environment.keycloak.clientId,
        },
        initOptions: {
          onLoad: 'check-sso',
          silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
        },
        enableBearerInterceptor: true,
        bearerPrefix: 'Bearer',
      });
  }

  return () => {
    return new Promise<Boolean>((resolve) => {
      resolve(true);
    });
  };
}
