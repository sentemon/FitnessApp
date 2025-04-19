import {
  AfterViewChecked, ChangeDetectionStrategy, ChangeDetectorRef,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {Message} from "../../models/message.model";
import {SignalRService} from "../../services/signalr.service";
import {CookieService} from "../../../../core/services/cookie.service";

@Component({
  selector: 'app-chat-area',
  templateUrl: './chat-area.component.html',
  styleUrl: './chat-area.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
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
    private cookieService: CookieService,
    private cdr: ChangeDetectorRef
  ) {
    this.currentUserId = this.cookieService.get("userId").response!;
  }

  ngOnInit(): void {
    this.tryLoadChat();

    this.signalRService.onReceiveMessage = (message: Message) => {
      if (this.selectedChat && message.chatId === this.selectedChat.id) {
        this.selectedChat.messages = [...(this.selectedChat.messages || []), message];
        console.log("msg added", message);
        this.cdr.markForCheck();
      }
    };
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["selectedChatId"] && !changes["selectedChatId"].firstChange) {
      this.tryLoadChat();
    }
  }

  ngAfterViewChecked() {
    if (this.messagesContainer) {
      this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
    }
  }

  sendMessage() {
    if (this.selectedChat) {
      this.signalRService.sendMessage(this.selectedChat.id, this.content.trim());
      this.content = '';
    }
  }

  get isOnline(): boolean {
    return this.selectedChat?.userChats.find(uc => uc.userId !== this.currentUserId)?.user.isOnline ?? false;
  }

  get chatName(): string {
    return this.selectedChat?.userChats.find(uc => uc.userId !== this.currentUserId)?.user.username ?? "Unknown";
  }

  get filteredMessages(): Message[] {
    if (!this.selectedChat || !this.selectedChat.messages) return [];

    return this.selectedChat.messages
      .filter(m => m.content.includes(this.searchQuery))
      .sort((a, b) => new Date(a.sentAt).getTime() - new Date(b.sentAt).getTime());
  }

  private tryLoadChat() {
    if (!this.selectedChatId) return;

    this.chatService.getById(this.selectedChatId).subscribe(result => {
      if (result.isSuccess) {
        this.selectedChat = result.response!;
        const token = this.cookieService.get("token").response!;
        this.signalRService.startConnection(this.selectedChat.id, token);
      } else {
        console.error(result.error.message);
      }
    });
  }
}
