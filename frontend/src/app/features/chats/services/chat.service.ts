import {Injectable} from '@angular/core';
import {Chat} from "../models/chat.model";
import {BehaviorSubject, map, Observable} from "rxjs";
import {Apollo, ApolloBase} from "apollo-angular";
import {GET_ALL_CHATS, GET_CHAT_BY_ID} from "../requests/queries.graphql";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chatsSubject = new BehaviorSubject<Chat[]>([]);
  private chats$ = this.chatsSubject.asObservable();

  private chatClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.chatClient = apollo.use("chats");
  }

  getAll(): Observable<Result<Chat[]>> {
    const result$ = this.chatClient.query({
      query: GET_ALL_CHATS
    }).pipe(
      toResult<Chat[]>("allChats")
    );

    result$.subscribe(result => {
      if (result.isSuccess) {
        this.chatsSubject.next(result.response);
      }
    });

    return result$;
  }

  get(chatId: string | null) {
    return this.chats$.pipe(
      map(chats => chats.find(c => c.id === chatId) ?? null)
    );
  }

  getById(chatId: string): Observable<Result<Chat>> {
    return this.chatClient.query({
      query: GET_CHAT_BY_ID,
      variables: { chatId },
      fetchPolicy: 'network-only'
    }).pipe(
      toResult<Chat>("chatById")
    );
  }
}
