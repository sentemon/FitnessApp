import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import {map, Observable} from "rxjs";
import {GET_USER_BY_USERNAME} from "../requests/queries";
import {User} from "../models/user.model";
import {UserResponse} from "../responses/user.response";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apollo: Apollo) { }

  getUserByUsername(username: string): Observable<User> {
    return this.apollo.query<UserResponse>({
      query: GET_USER_BY_USERNAME,
      variables: { username }
    }).pipe(
      map(response => response.data.userByUsername)
    );
  }
}
