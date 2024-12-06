import { Injectable } from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from 'rxjs';
import {KeycloakService} from "keycloak-angular";

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {

  constructor(private keycloakService: KeycloakService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return new Observable(observer => {
      this.keycloakService.updateToken(5).then(() => {
        const token = this.keycloakService.getKeycloakInstance().token;

        if (token) {
          req = req.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`,
            },
          });
        }

        next.handle(req).subscribe(
          event => observer.next(event),
          err => observer.error(err),
          () => observer.complete()
        );
      }).catch(err => {
        console.error('Failed to refresh token', err);
        observer.error(err);
      });
    });
  }

}
