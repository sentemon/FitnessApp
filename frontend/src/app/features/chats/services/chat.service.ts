import {Injectable} from '@angular/core';
import {Chat} from "../models/chat.model";
import {Observable, of} from "rxjs";
import {Apollo, ApolloBase} from "apollo-angular";
import {GET_ALL_CHATS} from "../requests/queries.graphql";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chats: Chat[] = [];

  private chatClient: ApolloBase

  constructor(apollo: Apollo) {
    this.chatClient = apollo.use("chats");
  }

  getAll(): Observable<Result<Chat[]>> {
    return this.chatClient.query({
      query: GET_ALL_CHATS
    }).pipe(
      toResult<Chat[]>("allChats")
    );
  }

  get(chatId: string | null): Observable<Chat | null> {
    return of(this.chats.find(c => c.id == chatId) ?? null)
  }
}
