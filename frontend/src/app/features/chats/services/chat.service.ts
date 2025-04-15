import { Injectable } from '@angular/core';
import {Chat} from "../models/chat.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chats: Chat[] = [
    {
      id: 'chat1',
      messages: [
        {
          id: '123',
          senderId: '123',
          sender: undefined!,
          chatId: 'chat1',
          content: 'zxc',
          sentAt: new Date()
        }
      ],
      users: []
    }
  ];

  constructor() { }

  getAll(): Observable<Chat[]> {
    return of(this.chats);
  }

  get(chatId: string | null): Observable<Chat | null> {
    return of(this.chats.find(c => c.id == chatId) ?? null)
  }
}
