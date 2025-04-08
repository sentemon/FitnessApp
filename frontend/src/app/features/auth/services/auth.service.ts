import {inject, Injectable} from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {BehaviorSubject, map, Observable} from "rxjs";
import {LOGIN, REGISTER} from "../requests/mutations";
import {MutationResponse} from "../responses/mutation.response";
import {CookieService} from "../../../core/services/cookie.service";
import {Result} from "../../../core/types/result/result.type";
import {catchError} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authClient: ApolloBase;

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.checkAuth());
  private isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(apollo: Apollo) {
    this.authClient = apollo.use("auth");
  }

  private checkAuth(): boolean {
    let cookieService = inject(CookieService);

    const token = cookieService.get("token").response;

    return token !== "There is no cookie with key token.";
  }

  public login(username: string, password: string): Observable<Result<boolean>> {
    return this.authClient.mutate<MutationResponse>({
      mutation: LOGIN,
      variables: { username, password },
    }).pipe(
      map(response => {
        const token = response.data?.login;

        if (token) {
          this.isAuthenticatedSubject.next(true);
          return Result.success(true);
        } else {
          this.isAuthenticatedSubject.next(false);
          console.error("Login failed: no token received.");
          return Result.success(false);
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
  ): Observable<Result<boolean>> {
    return this.authClient.mutate<MutationResponse>({
      mutation: REGISTER,
      variables: { firstName, lastName, username, email, password }
    }).pipe(
      map(response => {
        const token = response.data?.register;

        if (token) {
          this.isAuthenticatedSubject.next(true);
          return Result.success(true);
        } else {
          this.isAuthenticatedSubject.next(false);
          console.error("Registration failed: no token received.");
          return Result.success(false);
        }
      })
    );
  }


  public isAuthenticated(): Observable<Result<boolean>> {
    return this.isAuthenticated$.pipe(
      map(value => Result.success(value)),
    );
  }
}
