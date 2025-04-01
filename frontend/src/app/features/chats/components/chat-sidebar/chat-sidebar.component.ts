import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {User} from "../../models/user.model";

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrl: './chat-sidebar.component.scss'
})
export class ChatSidebarComponent implements OnInit {
  chats: Chat[] = [];
  searchTerm: string = '';

  @Output() selectedChatId = new EventEmitter<string>();

  constructor(private chatService: ChatService) { }

  ngOnInit() {
    this.chatService.getAll().subscribe(result => this.chats = result);
  }

  onSelectChat(chatId: string): void {
    this.selectedChatId.emit(chatId);
  }

  getChatName(chat: Chat): string {
    let result: User = {
      id: 'user1',
      firstName: 'Ivan',
      lastName: 'Sentemon',
      username: 'sentemon',
      isOnline: false,
      chats: []
    };

    return chat.users.find(u => u.username !== result.username)!.username;
  }

  get filteredChats(): Chat[] {
    return this.chats.filter(chat =>
      chat.users.some(user => user.username.toLowerCase().startsWith(this.searchTerm.toLowerCase()))
    );
  }
}
