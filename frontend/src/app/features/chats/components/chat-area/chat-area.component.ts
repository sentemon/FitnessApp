import {
  AfterViewChecked,
  Component,
  ElementRef,
  Input,
  OnChanges, OnDestroy,
  OnInit,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {Message} from "../../models/message.model";
import {SignalRService} from "../../services/signalr.service";
import {CookieService} from "../../../../core/services/cookie.service";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";
import {DeviceService} from "../../../../core/services/device.service";

@Component({
  selector: 'app-chat-area',
  templateUrl: './chat-area.component.html',
  styleUrl: './chat-area.component.scss',
})
export class ChatAreaComponent implements OnInit, OnChanges, AfterViewChecked, OnDestroy {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  selectedChat: Chat | null = null;
  currentUserId: string;
  searchQuery: string = '';
  content: string = '';

  @Input() selectedChatId: string | null = null;
  @Input() receiverId: string | null = null;

  private subscriptions = new Subscription();

  constructor(
    private chatService: ChatService,
    private signalRService: SignalRService,
    private cookieService: CookieService,
    protected deviceService: DeviceService,
    private router: Router,
  ) {
    this.currentUserId = this.cookieService.get("userId").response!;
  }

  ngOnInit(): void {
    if (this.selectedChatId) {
      const token = this.cookieService.get("token").response!;
      this.tryLoadChat();
      this.signalRService.stopConnection();
      this.signalRService.startConnection(this.selectedChatId, token);
    } else if (this.receiverId) {
      const token = this.cookieService.get("token").response!;
      this.signalRService.stopConnection();
      this.signalRService.startTempConnection(this.receiverId, token);
    }

    this.subscriptions.add(
      this.signalRService.onReceiveMessage.subscribe(message => {
        if (!this.selectedChat || this.selectedChat.id !== message.chatId) {
          this.chatService.getById(message.chatId).subscribe(result => {
            if (result.isSuccess) {
              this.selectedChat = structuredClone(result.response);
              this.router.navigate(['/chats', this.selectedChat.id]);
            }
          });
        } else {
          this.selectedChat.messages.push(message);
        }
      })
    );
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

  ngOnDestroy() {
    this.signalRService.stopConnection();
    this.subscriptions.unsubscribe();
  }

  sendMessage() {
    const message = this.content.trim();

    if (!message) return;

    if (this.selectedChat) {
      this.signalRService.sendMessage(
        this.selectedChat.userChats.find(u => u.userId !== this.currentUserId)!.userId, message
      );
      const oldMessageResult = this.chatService.getCachedLastMessage(this.selectedChat.id);

      const newMessage: Message = {
        ...oldMessageResult.response!,
        content: message,
        sentAt: new Date(),
      };

      this.chatService.updateLastMessage(this.selectedChat.id, newMessage);
    }
    else if (this.receiverId) {
      this.signalRService.sendMessage(this.receiverId, message);
    }

    this.content = '';
    this.scrollToBottom();
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
        this.selectedChat = structuredClone(result.response);
        const token = this.cookieService.get("token").response!;
        this.signalRService.startConnection(this.selectedChat.id, token);
      } else {
        console.log(result.error.message);
      }
    });
  }

  private scrollToBottom(): void {
    if (this.messagesContainer) {
      setTimeout(() => {
        const container = this.messagesContainer.nativeElement;
        container.scrollTop = container.scrollHeight;
      });
    }
  }

  showChats() {
    this.router.navigate(['/chats']);
  }
}
