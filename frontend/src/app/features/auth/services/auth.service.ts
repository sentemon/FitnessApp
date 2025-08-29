import {inject, Injectable} from '@angular/core';
import { Apollo, ApolloBase } from "apollo-angular";
import {BehaviorSubject, from, map, Observable, tap} from "rxjs";
import { LOGIN, LOGOUT, REGISTER } from "../graphql/mutations";
import { StorageService } from "../../../core/services/storage.service";
import { Result } from "../../../core/types/result/result.type";
import { UserService } from "./user.service";
import { toResult } from "../../../core/extensions/graphql-result-wrapper";
import {Token} from "../../../core/models/token.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authClient: ApolloBase;

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.checkAuth());
  private isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(apollo: Apollo, private userService: UserService, private storageService: StorageService) {
    this.authClient = apollo.use("auth");
  }

  private checkAuth(): boolean {
    let storageService = inject(StorageService);

    const result = storageService.getAccessToken();

    return result.isSuccess;
  }

  public login(username: string, password: string): Observable<Result<Token>> {
    return this.authClient.mutate({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      toResult<Token>("login"),
      tap(async result => {
        if (!result.isSuccess) {
          this.isAuthenticatedSubject.next(false);
          console.log("Login failed:", result.error.message);
          return Result.failure(new Error(result.error.message));
        }

        await this.setUserCookies(result.response.accessToken, result.response.refreshToken);
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
  ): Observable<Result<Token>> {
    return this.authClient.mutate({
      mutation: REGISTER,
      variables: { firstName, lastName, username, email, password }
    }).pipe(
      toResult<Token>("register"),
      tap(async result => {
        if (!result.isSuccess) {
          this.isAuthenticatedSubject.next(false);
          console.log("Registration failed:", result.error.message);
          return Result.failure(new Error(result.error.message));
        }

        await this.setUserCookies(result.response.accessToken, result.response.refreshToken);
        this.isAuthenticatedSubject.next(true);

        return Result.success("Successfully registered");
      }),
      map(result => {
        return result.isSuccess ? Result.success(result.response) : Result.failure(new Error(result.error.message));
      })
    );
  }

  public logout(): Observable<Result<string>> {
    const refreshToken = this.storageService.getRefreshToken();

    this.storageService.delete("token");
    this.storageService.delete("refreshToken");
    this.storageService.delete("userId");
    this.storageService.delete("username");

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

  private async setUserCookies(accessToken: string, refreshToken: string): Promise<void> {
    await this.storageService.set("token", accessToken, 1);
    await this.storageService.set("refreshToken", refreshToken, 1);

    this.userService.getCurrentUser().subscribe(async result => {
      if (result.isSuccess) {
        await this.storageService.set("userId", result.response.id, 1);
        await this.storageService.set("username", result.response.username.value, 1);
      }
    });
  }
}
