import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { KeycloakAuthGuard, KeycloakService } from "keycloak-angular";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard extends KeycloakAuthGuard {

  constructor(
    protected override router: Router,
    protected override keycloakAngular: KeycloakService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    super(router, keycloakAngular);
  }

  public override async isAccessAllowed(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> {
    if (isPlatformBrowser(this.platformId)) {
      if (!this.authenticated) {
        await this.keycloakAngular.login({
          redirectUri: window.location.origin + state.url
        });
        return false;
      }
      return true;
    }
    return false;
  }
}
