import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import { Observable, of } from "rxjs";
import {GET_CURRENT_USER, GET_FOLLOWERS, GET_USER_BY_USERNAME, SEARCH_USERS} from "../requests/queries";
import {User} from "../models/user.model";
import {QueryResponses} from "../responses/query.responses";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";
import {UserDto} from "../models/user-dto.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.authClient = apollo.use("auth");
  }

  getCurrentUser(): Observable<Result<User>> {
    return this.authClient.query<QueryResponses>({
      query: GET_CURRENT_USER
    }).pipe(
      toResult<User>('currentUser')
    );
  }

  getUserByUsername(username: string): Observable<Result<UserDto>> {
    return this.authClient.query<QueryResponses>({
      query: GET_USER_BY_USERNAME,
      variables: { username }
    }).pipe(
      toResult<UserDto>('userByUsername')
    );
  }

  searchUsers(query: string): Observable<Result<UserDto[]>> {
    return this.authClient.query({
      query: SEARCH_USERS,
      variables: { search: query }
    }).pipe(
      toResult<UserDto[]>('searchUsers')
    );
  }

  getFollowers(): Observable<Result<User[]>> {
    return this.authClient.query({
      query: GET_FOLLOWERS
    }).pipe(
      toResult<User[]>('followers')
    );
  }
}
