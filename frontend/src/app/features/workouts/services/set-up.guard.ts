import {Injectable} from "@angular/core";
import {
  CanActivate, Router,
} from '@angular/router';
import {UserService} from "./user.service";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SetUpGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) { }

  canActivate(): Observable<boolean> {
    return this.userService.profileSetUp().pipe(
      map(result => {
        if (result) {
          return true;
        }

        this.router.navigate(['/setup-profile']);
        return false;
      })
    )
  }
}
