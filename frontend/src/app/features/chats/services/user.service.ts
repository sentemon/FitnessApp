import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {Observable} from "rxjs";
import {Result} from "../../../core/types/result/result.type";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {User} from "../models/user.model";
import {SEARCH_USERS} from "../graphql/queries.graphql";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private chatClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.chatClient = apollo.use("chats");
  }

  searchUsers(query: string): Observable<Result<User[]>> {
    return this.chatClient.query({
      query: SEARCH_USERS,
      variables: { search: query }
    }).pipe(
      toResult<User[]>('searchUsers')
    );
  }
}
