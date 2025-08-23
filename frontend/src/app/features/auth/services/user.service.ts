import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {Observable, of} from "rxjs";
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
import {DELETE_USER, FOLLOW, RESET_PASSWORD, UNFOLLOW, UPDATE_USER} from "../graphql/mutations";
import {environment} from "../../../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {print} from "graphql/language";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authClient: ApolloBase;

  constructor(apollo: Apollo, private http: HttpClient) {
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

  // update(firstName: string, lastName: string, username: string, email: string, image: File | null): Observable<Result<string>> {
  //   return this.authClient.mutate({
  //     mutation: UPDATE_USER,
  //     variables: { firstName, lastName, username, email, image }
  //   }).pipe(
  //     toResult<string>('updateUser')
  //   );
  // }

  update(
    firstName: string,
    lastName: string,
    username: string,
    email: string,
    bio: string,
    image: File | null
  ): Observable<any> {
    const formData = new FormData();

    const operations = {
      query: print(UPDATE_USER),
      variables: {
        firstName,
        lastName,
        username,
        email,
        bio,
        image: null
      }
    };

    formData.append("operations", JSON.stringify(operations));

    if (image) {
      const map = { "0": ["variables.image"] };
      formData.append("map", JSON.stringify(map));
      formData.append("0", image);
    } else {
      formData.append("map", JSON.stringify({}));
    }

    return this.http.post(environment.auth_service, formData, {
      headers: new HttpHeaders().set("GraphQL-Preflight", "true")
    });
  }


  resetPassword(oldPassword: string, newPassword: string, confirmNewPassword: string): Observable<Result<string>> {
    return this.authClient.mutate({
      mutation: RESET_PASSWORD,
      variables: { oldPassword, newPassword, confirmNewPassword }
    }).pipe(
      toResult<string>('resetPassword')
    );
  }

  delete(): Observable<Result<boolean>> {
    return this.authClient.mutate({
      mutation: DELETE_USER
    }).pipe(
      toResult<boolean>('deleteUser')
    );
  }
}
