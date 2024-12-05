import { KeycloakService } from 'keycloak-angular';

const Keycloak = typeof window !== 'undefined' ? import('keycloak-js') : null;

export function initKeycloak(keycloak: KeycloakService) {
  if (Keycloak !== null) {
    return () =>
      keycloak.init({
        config: {
          url: 'http://localhost:8080/',
          realm: 'keycloak-angular-realm',
          clientId: 'keycloak-angular-client'
        },
        initOptions: {
          onLoad: 'login-required',
          checkLoginIframe: false,
        },
        // enableBearerInterceptor: true,
        // bearerPrefix: 'Bearer',
      }).then(authenticated => {
        console.log('Authenticated:', authenticated);
        console.log('Token:', keycloak.getToken());
      }).catch(error => {
        console.log('Keycloak init error:', error);
      });
  } else {
    return () => {
      return new Promise<Boolean>((resolve) => {
        resolve(true);
      });
    };
  }
}
