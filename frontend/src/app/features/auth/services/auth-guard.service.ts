import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import { map } from "rxjs";
import {LOGIN, REGISTER} from "../requests/mutations";
import {Token} from "../../../core/models/token.model";
import {TokenResponse} from "../responses/token.response";
import {TokenService} from "../../../core/services/token.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService {
  private token?: Token;

  constructor(private apollo: Apollo, private tokenService: TokenService) { }

  public login(username: string, password: string): void {
    this.apollo.mutate<TokenResponse>({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      map(response => response.data?.login)
    ).subscribe(result => {
      this.token = result;
    });

    this.tokenService.set(this.token);
  }

  public register(
    firstName: string,
    lastName: string,
    username: string,
    email: string,
    password: string
  ): void {
    this.apollo.mutate<TokenResponse>({
      mutation: REGISTER,
      variables: { firstName, lastName, username, email, password }
    }).pipe(
      map(response => response.data?.register)
    ).subscribe(result => {
      this.token = result;
    });

    this.tokenService.set(this.token);
  }
}
