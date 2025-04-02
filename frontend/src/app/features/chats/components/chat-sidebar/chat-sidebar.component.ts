import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrl: './chat-sidebar.component.scss'
})
export class ChatSidebarComponent implements OnInit {
  currentUser!: User;
  chats: Chat[] = [];
  searchTerm: string = '';

  @Output() selectedChatId = new EventEmitter<string>();

  constructor(private chatService: ChatService, private userService: UserService) { }

  ngOnInit() {
    this.userService.getCurrent().subscribe(result => {
      this.currentUser = result;
      this.chatService.getAll().subscribe(result => this.chats = result);
    });
  }

  onSelectChat(chatId: string): void {
    this.selectedChatId.emit(chatId);
  }

  getChatName(chat: Chat): string {
    return chat.users.find(u => u.username !== this.currentUser.username)!.username;
  }

  get filteredChats(): Chat[] {
    return this.chats.filter(chat =>
      chat.users.some(user => user.username.toLowerCase().startsWith(this.searchTerm.toLowerCase()))
    );
  }
}
