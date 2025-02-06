import {Injectable} from "@angular/core";
import {
  CanActivate, Router,
} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SetUpGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(): boolean {
    if (this.isProfileComplete()) {
      return true;
    }

    this.router.navigate(['/setup-profile']);
    return false;
  }

  private isProfileComplete() {
    return false;
  }
}
