import {Injectable} from '@angular/core';
import {Apollo} from "apollo-angular";
import {map} from "rxjs";
import {LOGIN, REGISTER} from "../requests/mutations";
import {TokenResponse} from "../responses/token.response";
import {TokenService} from "../../../core/services/token.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService {

  constructor(private apollo: Apollo, private tokenService: TokenService) { }

  public login(username: string, password: string): void {
    this.apollo.mutate<TokenResponse>({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      map(response => response.data?.login)
    ).subscribe(result => {
      if (result) {
        this.tokenService.set(result);
      } else {
        console.error("Login failed: no token received.");
      }
    });
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
      if (result) {
        this.tokenService.set(result);
      } else {
        console.error("Register failed: no token received.");
      }
    });
  }
}
