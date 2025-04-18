import {AfterViewChecked, Component, ElementRef, Input, OnChanges, OnInit, ViewChild} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {Message} from "../../models/message.model";
import {SignalRService} from "../../services/signalr.service";
import {CookieService} from "../../../../core/services/cookie.service";

@Component({
  selector: 'app-chat-area',
  templateUrl: './chat-area.component.html',
  styleUrl: './chat-area.component.scss'
})
export class ChatAreaComponent implements OnInit, OnChanges, AfterViewChecked {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  selectedChat: Chat | null = null;
  currentUserId: string;
  searchQuery: string = '';
  content: string = '';

  @Input() selectedChatId: string | null = null;

  constructor(
    private chatService: ChatService,
    private signalRService: SignalRService,
    private cookieService: CookieService
  ) {
    this.currentUserId = this.cookieService.get("userId").response!;
  }


  ngOnInit(): void {
    this.chatService.get(this.selectedChatId).subscribe(result => {
      this.selectedChat = result;
    })

    const result = this.cookieService.get("token");
    if (result.isSuccess) {
      this.signalRService.onReceiveMessage = msg => {
        // this.
        console.log('New message received:', msg);
      };

      if (this.selectedChat) {
        this.signalRService.startConnection(this.selectedChat.id, result.response);
      }
    } else {
      console.log(result.error.message);
    }
  }

  ngOnChanges() {
    if (this.selectedChat) {
      this.chatService.get(this.selectedChat.id).subscribe(result => this.selectedChat = result)
    }
  }

  ngAfterViewChecked() {
    if (!this.messagesContainer)
      return;

    this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
  }

  sendMessage() {
    if (this.selectedChat) {
      this.signalRService.sendMessage(this.selectedChat.id, this.content);

      this.content = '';
    }
  }

  get isOnline(): boolean {
    return this.selectedChat?.userChats.find(uc => uc.userId !== this.currentUserId)!.user.isOnline ?? false;
  }

  get chatName(): string {
    return this.selectedChat?.userChats.find(uc => uc.userId !== this.currentUserId)!.user.username ?? "Unknown";
  }

  get filteredMessages(): Message[] {
    return this.selectedChat?.messages.filter(m => m.content.includes(this.searchQuery)) ?? [];
  }
}
