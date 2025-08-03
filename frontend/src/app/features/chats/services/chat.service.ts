import {Injectable} from '@angular/core';
import {Chat} from "../models/chat.model";
import {BehaviorSubject, map, Observable} from "rxjs";
import {Apollo, ApolloBase} from "apollo-angular";
import {GET_ALL_CHATS, GET_CHAT_BY_ID, GET_LAST_MESSAGE} from "../graphql/queries.graphql";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";
import {Message} from "../models/message.model";
import {CustomError} from "../../../core/types/result/custom-error.type";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chatsSubject = new BehaviorSubject<Chat[]>([]);
  private chats$ = this.chatsSubject.asObservable();
  private lastMessagesMap = new Map<string, Message>();
  private lastMessages$ = new BehaviorSubject<Map<string, Message>>(this.lastMessagesMap);

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

  getById(chatId: string): Observable<Result<Chat>> {
    return this.chatClient.query({
      query: GET_CHAT_BY_ID,
      variables: { chatId },
      fetchPolicy: 'network-only'
    }).pipe(
      toResult<Chat>("chatById")
    );
  }

  fetchLastMessage(chatId: string): Observable<Result<Message>> {
    return this.chatClient.query({
      query: GET_LAST_MESSAGE,
      variables: { chatId }
    }).pipe(
      toResult<Message>("lastMessage"),
    );
  }

  updateLastMessage(chatId: string, message: Message): void {
    this.lastMessagesMap.set(chatId, message);
    this.lastMessages$.next(new Map(this.lastMessagesMap));
  }

  getCachedLastMessage(chatId: string): Result<Message> {
    const lastMessage = this.lastMessagesMap.get(chatId);
    if (!lastMessage) {
      return Result.failure(new Error("Last message not found."));
    }

    return Result.success(lastMessage)
  }
}
