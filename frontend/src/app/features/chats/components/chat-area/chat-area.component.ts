import {AfterViewChecked, Component, ElementRef, Input, OnChanges, ViewChild} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";
import {Message} from "../../models/message.model";

@Component({
  selector: 'app-chat-area',
  templateUrl: './chat-area.component.html',
  styleUrl: './chat-area.component.scss'
})
export class ChatAreaComponent implements OnChanges, AfterViewChecked {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  selectedChat: Chat | null = null;
  currentUser!: User;
  searchQuery: string = '';

  @Input() selectedChatId: string | null = null;

  constructor(private chatService: ChatService, private userService: UserService) { }

  ngOnChanges() {
    this.userService.getCurrent().subscribe(result => {
      this.currentUser = result;
      this.chatService.get(this.selectedChatId).subscribe(result => this.selectedChat = result);
    });
  }

  ngAfterViewChecked() {
    if (!this.messagesContainer)
      return;

    this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
  }

  get isOnline(): boolean {
    return this.selectedChat!.users.find(u => u.id !== this.currentUser.id)!.isOnline;
  }

  get chatName(): string {
    return this.selectedChat!.users.find(u => u.username !== this.currentUser.username)!.username;
  }

  get filteredMessages(): Message[] {
    return this.selectedChat!.messages.filter(m => m.content.includes(this.searchQuery));
  }

  protected readonly navigator = navigator;
}
