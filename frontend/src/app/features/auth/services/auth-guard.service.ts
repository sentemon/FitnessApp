import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import {map, Observable} from "rxjs";
import {LOGIN, REGISTER} from "../requests/mutations";
import {Token} from "../../../core/models/token.model";
import {TokenResponse} from "../responses/token.response";

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService {

  constructor(private apollo: Apollo) { }

  public login(username: string, password: string) : Observable<Token | undefined> {
    return this.apollo.mutate<TokenResponse>({
      mutation: LOGIN,
      variables: { username, password }
    }).pipe(
      map(response => response.data?.login)
    );
  }

  public register(firstName: string, lastName: string, username: string, email: string, password: string) : Observable<Token | undefined> {
    return this.apollo.mutate<TokenResponse>({
      mutation: REGISTER,
      variables: { firstName, lastName, username, email, password }
    }).pipe(
      map(response => response.data?.register)
    );
  }
}
