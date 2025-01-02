import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { KeycloakAuthGuard, KeycloakService } from "keycloak-angular";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor() {}
}
