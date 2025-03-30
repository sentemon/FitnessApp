import { Injectable } from '@angular/core';
import {Chat} from "../models/chat.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chats: Chat[] = [
    {
      id: "chat1",
      messages: [
        {
          id: '1',
          content: 'Hi, how are you?',
          createdAt: new Date(),
          chatId: 'chat1',
          userId: 'user1',
        },
        {
          id: '2',
          content: "Hi, I'm good, what about you?",
          createdAt: new Date(),
          chatId: 'chat1',
          userId: 'user2',
        }
      ],
      users: [
        {
          id: 'user1',
          firstName: 'Ivan',
          lastName: 'Sentemon',
          username: 'sentemon',
          isOnline: true,
          chats: []
        },
        {
          id: 'user2',
          firstName: 'Navi',
          lastName: 'Nometnes',
          username: 'nometnes',
          isOnline: false,
          chats: []
        }
      ],
    },
    {
      id: "chat2",
      messages: [
        {
          id: '1',
          content: 'Hi, how are you?',
          createdAt: new Date(),
          chatId: 'chat1',
          userId: 'user1',
        },
        {
          id: '2',
          content: "Hi, I'm good, what about you?",
          createdAt: new Date(),
          chatId: 'chat1',
          userId: 'user2',
        }
      ],
      users: [
        {
          id: 'user1',
          firstName: 'Ivan',
          lastName: 'Sentemon',
          username: 'sentemon',
          isOnline: true,
          chats: []
        },
        {
          id: 'user2',
          firstName: 'Navi',
          lastName: 'Nometnes',
          username: 'nometnes',
          isOnline: false,
          chats: []
        }
      ],
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
