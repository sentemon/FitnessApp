import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import {Observable} from "rxjs";
import {GET_USER_BY_USERNAME} from "../requests/queries";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apollo: Apollo) { }

  getUserByUsername(username: string): Observable<any> {
    return this.apollo.query({
      query: GET_USER_BY_USERNAME,
      variables: { username }
    });
  }
}
