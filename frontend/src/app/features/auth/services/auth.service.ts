import {Injectable} from '@angular/core';
import {Apollo} from "apollo-angular";
import {map, Observable} from "rxjs";
import {LOGIN, REGISTER} from "../requests/mutations";
import {TokenService} from "../../../core/services/token.service";
import {MutationResponse} from "../responses/mutation.response";
import {QueryResponses} from "../responses/query.responses";
import {IS_AUTHENTICATED} from "../requests/queries";
import {environment} from "../../../../environments/environment";
import {InMemoryCache} from "@apollo/client/core";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private apollo: Apollo, private tokenService: TokenService) {
    // apollo.create({
    //   uri: environment.auth_service,
    //   cache: new InMemoryCache()
    // });
  }

  public login(username: string, password: string): Observable<boolean> {
    return this.apollo.mutate<MutationResponse>({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      map(response => {
        const token = response.data?.login;

        if (token) {
          this.tokenService.set(token);
          return true;
        } else {
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
          this.tokenService.set(token);
          return true;
        } else {
          console.error("Registration failed: no token received.");
          return false;
        }
      })
    );
  }


  public isAuthenticated(): Observable<boolean> {
    return this.apollo.query<QueryResponses>({
      query: IS_AUTHENTICATED
    }).pipe(
      map(response => response.data.isAuthenticated));
  }
}
