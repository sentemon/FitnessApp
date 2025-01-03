import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import {map, Observable} from "rxjs";
import {LOGIN} from "../requests/mutations";
import {Token} from "../models/token.model";
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
}
