import { Injectable } from '@angular/core';
import {User} from "../models/user.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  users: User[] = [
    {
      id: 'user1',
      firstName: 'Ivan',
      lastName: 'Sentemon',
      username: 'sentemon',
      email: '',
      isOnline: false,
      chats: []
    },
    {
      id: 'user2',
      firstName: 'Ivan',
      lastName: 'Babachov',
      username: 'babachov',
      email: '',
      isOnline: true,
      chats: []
    },
    {
      id: 'user3',
      firstName: 'Deni',
      lastName: 'Gabedava',
      username: 'gabedava',
      email: '',
      isOnline: false,
      chats: []
    },
    {
      id: 'user4',
      firstName: 'Valera',
      lastName: 'Star',
      username: 'coolman',
      email: '',
      isOnline: true,
      chats: []
    },
  ];

  constructor() { }

  getCurrent(): Observable<User> {
    return of(this.users.find(u => u.username === "sentemon")!);
  }
}
