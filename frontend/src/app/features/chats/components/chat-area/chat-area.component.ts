import {AfterViewChecked, Component, ElementRef, Input, OnChanges, OnInit, ViewChild} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";
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
  currentUser!: User;
  searchQuery: string = '';
  content: string = '';

  @Input() selectedChatId: string | null = null;

  constructor(
    private chatService: ChatService,
    private userService: UserService,
    private signalRService: SignalRService,
    private cookieService: CookieService
  ) { }

  ngOnInit(): void {
    const result = this.cookieService.get("token").response;

    this.signalRService.onReceiveMessage = msg => {
      console.log('New message received:', msg);
    };

    if (this.selectedChat) {
      this.signalRService.startConnection(this.selectedChatId!, result!);
    }
  }

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

  sendMessage() {
    this.signalRService.sendMessage(this.selectedChatId!, this.content);

    this.content = '';
  }

  get isOnline(): boolean {
    return this.selectedChat!.userChats.find(uc => uc.userId !== this.currentUser.id)!.user.isOnline;
  }

  get chatName(): string {
    return this.selectedChat!.userChats.find(uc => uc.userId !== this.currentUser.id)!.user.username;
  }

  get filteredMessages(): Message[] {
    return this.selectedChat!.messages.filter(m => m.content.includes(this.searchQuery));
  }

  protected readonly navigator = navigator;
}
