import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {CookieService} from "../../../../core/services/cookie.service";
import {FormControl} from "@angular/forms";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";
import {debounceTime, distinctUntilChanged, switchMap} from "rxjs";

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrl: './chat-sidebar.component.scss'
})
export class ChatSidebarComponent implements OnInit {
  currentUsername: string;
  chats: Chat[] = [];
  users: User[] = [];
  searchControl = new FormControl('');

  @Output() selectedChatId = new EventEmitter<string>();
  @Output() userSelected = new EventEmitter<string>();

  constructor(
    private chatService: ChatService,
    private userService: UserService,
    cookieService: CookieService) {
    this.currentUsername = cookieService.get("username").response!;
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => this.userService.searchUsers(query || ''))
    ).subscribe(result => this.users = result?.response ?? []);
  }

  ngOnInit() {
    this.chatService.getAll().subscribe(result => {
      console.log(result);
      if (result.isSuccess)
        this.chats = result.response;
    });
  }

  onSelectChat(chatId: string): void {
    this.selectedChatId.emit(chatId);
  }

  onUserSelected(userId: string) {
    this.userSelected.emit(userId);
  }

  getChatName(chat: Chat): string {
    return chat.userChats.find(uc => uc.user.username !== this.currentUsername)!.user.username;
  }

  // get filteredChats(): Chat[] {
  //   return this.chats.filter(chat =>
  //     chat.userChats.some(uc => uc.user.username.toLowerCase().startsWith(this.searchControl.value!.toLowerCase()) && uc.user.username !== this.currentUsername)
  //   );
  // }
}
