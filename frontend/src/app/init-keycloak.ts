import { KeycloakService } from 'keycloak-angular';
import {environment} from "../environments/environment";

const Keycloak = typeof window !== 'undefined' ? import('keycloak-js') : null;

export function initKeycloak(keycloak: KeycloakService) {
  if (environment.devMode) {
    return () => new Promise<boolean>((resolve) => resolve(true));
  }

  else if (Keycloak !== null) {
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
