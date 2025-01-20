import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {map, Observable} from "rxjs";
import {GET_CURRENT_USER, GET_USER_BY_USERNAME} from "../requests/queries";
import {User} from "../models/user.model";
import {QueryResponses} from "../responses/query.responses";
import {ApolloLink} from "@apollo/client/core";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authClient: ApolloBase;

  constructor(private apollo: Apollo) {
    this.authClient = apollo.use("auth");
  }

  getCurrentUser(): Observable<User> {
    return this.authClient.query<QueryResponses>({
      query: GET_CURRENT_USER
    }).pipe(
      map(response => response.data.currentUser)
    );
  }

  getUserByUsername(username: string): Observable<User> {
    return this.apollo.query<QueryResponses>({
      query: GET_USER_BY_USERNAME,
      variables: { username }
    }).pipe(
      map(response => response.data.userByUsername)
    );
  }
}
