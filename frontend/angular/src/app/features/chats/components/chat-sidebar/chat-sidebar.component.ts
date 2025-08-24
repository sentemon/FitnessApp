import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {CookieService} from "../../../../core/services/cookie.service";
import {FormControl} from "@angular/forms";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";
import {debounceTime, distinctUntilChanged, switchMap} from "rxjs";
import {Message} from "../../models/message.model";
import {DateService} from "../../../../core/services/date.service";

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
    private dateService: DateService,
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
      if (result.isSuccess) {
        this.chats = result.response;
      } else {
        console.log(result.error.message);
      }

      for (const chat of this.chats) {
        this.chatService.fetchLastMessage(chat.id).subscribe(result => {
          if (result.isSuccess) {
            this.chatService.updateLastMessage(chat.id, result.response);
          } else {
            console.log(result.error.message);
          }
        })
      }
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

  getLastMessage(chatId: string): { content: string, sentAt: string } {
    const result = this.chatService.getCachedLastMessage(chatId);

    if (!result.isSuccess) {
      return {
        content: "Loading...",
        sentAt: ""
      };
    }

    return {
      content: result.response.content,
      sentAt: this.dateService.formatMessageDate(result.response.sentAt)
    };
  }

  get filteredChats(): Chat[] {
    return this.chats.slice().sort((a, b) => {
      const aResult = this.chatService.getCachedLastMessage(a.id);
      const bResult = this.chatService.getCachedLastMessage(b.id);

      if (!aResult.isSuccess || !bResult.isSuccess) {
        return 0;
      }

      const aTime = new Date(aResult.response.sentAt).getTime();
      const bTime = new Date(bResult.response.sentAt).getTime();

      return bTime - aTime;
    });
  }

}
