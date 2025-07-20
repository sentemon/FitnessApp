import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import { Observable } from "rxjs";
import {
  GET_CURRENT_USER,
  GET_FOLLOWERS,
  GET_FOLLOWING,
  GET_USER_BY_USERNAME,
  IS_FOLLOWING,
  SEARCH_USERS
} from "../graphql/queries";
import {User} from "../models/user.model";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";
import {UserDto} from "../models/user-dto.model";
import {FOLLOW, UNFOLLOW} from "../graphql/mutations";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.authClient = apollo.use("auth");
  }

  getCurrentUser(): Observable<Result<User>> {
    return this.authClient.query({
      query: GET_CURRENT_USER
    }).pipe(
      toResult<User>('currentUser')
    );
  }

  getUserByUsername(username: string): Observable<Result<UserDto>> {
    return this.authClient.query({
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

  getFollowers(userId: string): Observable<Result<User[]>> {
    return this.authClient.query({
      query: GET_FOLLOWERS,
      variables: { userId }
    }).pipe(
      toResult<User[]>('followers')
    );
  }

  getFollowing(userId: string): Observable<Result<User[]>> {
    return this.authClient.query({
      query: GET_FOLLOWING,
      variables: { userId }
    }).pipe(
      toResult<User[]>('following')
    );
  }

  follow(targetUserId: string): Observable<Result<string>> {
    return this.authClient.mutate({
      mutation: FOLLOW,
      variables: { targetUserId }
    }).pipe(
      toResult<string>('follow')
    );
  }

  unfollow(targetUserId: string): Observable<Result<string>> {
    return this.authClient.mutate({
      mutation: UNFOLLOW,
      variables: { targetUserId }
    }).pipe(
      toResult<string>('unfollow')
    );
  }

  isFollowing(targetUserId: string): Observable<Result<boolean>> {
    return this.authClient.query({
      query: IS_FOLLOWING,
      variables: { targetUserId }
    }).pipe(
      toResult<boolean>('isFollowing')
    );
  }
}
