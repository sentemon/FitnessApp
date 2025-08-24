import {Injectable} from "@angular/core";
import {
  CanActivate, Router,
} from '@angular/router';
import {UserService} from "./user.service";
import {map, Observable, tap} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SetUpGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) { }

  canActivate(): Observable<boolean> {
    return this.userService.profileSetUp().pipe(
      tap(result => {
        if (!result.isSuccess) {
          this.router.navigate(['/setup-profile']);
        }
      }),
      map(result => result.isSuccess)
    );
  }
}
