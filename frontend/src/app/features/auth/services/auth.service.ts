import {inject, Injectable} from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {BehaviorSubject, map, Observable, tap} from "rxjs";
import {LOGIN, LOGOUT, REGISTER} from "../graphql/mutations";
import {CookieService} from "../../../core/services/cookie.service";
import {Result} from "../../../core/types/result/result.type";
import {UserService} from "./user.service";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authClient: ApolloBase;

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.checkAuth());
  private isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(apollo: Apollo, private userService: UserService, private cookieService: CookieService) {
    this.authClient = apollo.use("auth");
  }

  private checkAuth(): boolean {
    let cookieService = inject(CookieService);

    const result = cookieService.get("token");

    return result.isSuccess;
  }

  public login(username: string, password: string): Observable<Result<string>> {
    return this.authClient.mutate({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      toResult<string>("login"),
      tap(result => {
        if (!result.isSuccess) {
          this.isAuthenticatedSubject.next(false);
          console.log("Login failed:", result.error.message);
          return Result.failure(new Error(result.error.message));
        }

        this.setUserCookies();
        this.isAuthenticatedSubject.next(true);

        return Result.success("Successfully logged in");
      }),
      map(result => {
        return result.isSuccess ? Result.success(result.response) : Result.failure(new Error(result.error.message));
      })
    );
  }

  public register(
    firstName: string,
    lastName: string,
    username: string,
    email: string,
    password: string
  ): Observable<Result<string>> {
    return this.authClient.mutate({
      mutation: REGISTER,
      variables: { firstName, lastName, username, email, password }
    }).pipe(
      toResult<string>("register"),
      tap(result => {
        if (!result.isSuccess) {
          this.isAuthenticatedSubject.next(false);
          console.log("Registration failed:", result.error.message);
          return Result.failure(new Error(result.error.message));
        }

        this.setUserCookies();
        this.isAuthenticatedSubject.next(true);

        return Result.success("Successfully registered");
      }),
      map(result => {
        return result.isSuccess ? Result.success(result.response) : Result.failure(new Error(result.error.message));
      })
    );
  }

  public logout(): Observable<Result<string>> {
    this.cookieService.delete("userId");
    this.cookieService.delete("username");

    const refreshToken = this.cookieService.get("refreshToken").response!;

    return this.authClient.mutate({
      mutation: LOGOUT,
      variables: { refreshToken }
    }).pipe(
      toResult<string>("logout")
    );
  }


  public isAuthenticated(): Observable<Result<boolean>> {
    return this.isAuthenticated$.pipe(
      map(value => Result.success(value)),
    );
  }

  private setUserCookies(): void {
    this.userService.getCurrentUser().subscribe(result => {

      if (result.isSuccess) {
        console.log(result.response);
        this.cookieService.set("userId", result.response.id, 1);
        this.cookieService.set("username", result.response.username.value, 1);
      }
    });
  }
}
