import { Injectable } from '@angular/core';
import {User} from "../models/user.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor() { }

  getCurrent(): Observable<User> {
    const user: User = {
      id: '123',
      firstName: 'Ivan',
      lastName: 'Sentemon',
      username: 'sentemon1',
      email: '',
      isOnline: false,
      userChats: [
        {
          userId: '123',
          user: undefined!,
          chatId: 'chat1',
          chat: {
            id: 'chat1',
            messages: [],
            userChats: [],
            user: undefined!
          }
        }
      ]
    }

    return of(user);
  }
}
