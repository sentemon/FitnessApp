import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import {map, Observable} from "rxjs";
import {GET_USER_BY_USERNAME} from "../requests/queries";
import {User} from "../models/user.model";
import {QueryResponses} from "../responses/query.responses";
import {ApolloLink} from "@apollo/client/core";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apollo: Apollo) {
    // apollo.client.setLink(ApolloLink.from())
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
