import {inject, Injectable} from '@angular/core';
import {Apollo} from "apollo-angular";
import {BehaviorSubject, map, Observable} from "rxjs";
import {LOGIN, REGISTER} from "../requests/mutations";
import {MutationResponse} from "../responses/mutation.response";
import {CookieService} from "../../../core/services/cookie.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.checkAuth());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private apollo: Apollo) { }

  private checkAuth(): boolean {
    let cookieService = inject(CookieService);

    const token = cookieService.get("token");

    return token != "There is no cookie with key token.";
  }

  public login(username: string, password: string): Observable<boolean> {
    return this.apollo.mutate<MutationResponse>({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      map(response => {
        const token = response.data?.login;

        if (token) {
          this.isAuthenticatedSubject.next(true);
          return true;
        } else {
          this.isAuthenticatedSubject.next(false);
          console.error("Login failed: no token received.");
          return false;
        }
      })
    );
  }

  public register(
    firstName: string,
    lastName: string,
    username: string,
    email: string,
    password: string
  ): Observable<boolean> {
    return this.apollo.mutate<MutationResponse>({
      mutation: REGISTER,
      variables: { firstName, lastName, username, email, password }
    }).pipe(
      map(response => {
        const token = response.data?.register;

        if (token) {
          return true;
        } else {
          console.error("Registration failed: no token received.");
          return false;
        }
      })
    );
  }


  public isAuthenticated(): Observable<boolean> {
    return this.isAuthenticated$;
  }
}
