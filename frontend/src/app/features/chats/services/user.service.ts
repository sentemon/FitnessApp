import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private chatClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.chatClient = apollo.use("chats");
  }
}
