import { Injectable } from '@angular/core';
import {AuthService} from "../../features/auth/services/auth.service";
import {
  CanActivate,
  Router
} from "@angular/router";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(): Observable<boolean> {
    return this.authService.isAuthenticated().pipe(
      map(result => {
        if (result) {
          return true;
        } else {
          console.log(result);
          this.router.navigate(["login"])
          return false;
        }
      })
    );
  }
}
