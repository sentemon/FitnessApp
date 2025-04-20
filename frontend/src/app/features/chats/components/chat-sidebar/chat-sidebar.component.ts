import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";
import {CookieService} from "../../../../core/services/cookie.service";

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrl: './chat-sidebar.component.scss'
})
export class ChatSidebarComponent implements OnInit {
  currentUsername: string;
  chats: Chat[] = [];
  searchTerm: string = '';

  @Output() selectedChatId = new EventEmitter<string>();

  constructor(private chatService: ChatService, cookieService: CookieService) {
    this.currentUsername = cookieService.get("username").response!;
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

  getChatName(chat: Chat): string {
    return chat.userChats.find(uc => uc.user.username !== this.currentUsername)!.user.username;
  }

  get filteredChats(): Chat[] {
    return this.chats.filter(chat =>
      chat.userChats.some(uc => uc.user.username.toLowerCase().startsWith(this.searchTerm.toLowerCase()) && uc.user.username !== this.currentUsername)
    );
  }
}
