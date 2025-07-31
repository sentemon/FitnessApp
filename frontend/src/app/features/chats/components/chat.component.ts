import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {ChatService} from "../services/chat.service";
import {CookieService} from "../../../core/services/cookie.service";

@Component({
  selector: 'app-chat',
  template: `
    <div class="chat flex">
<!--   ToDo: disable *ngIf for desktop   -->
      <app-chat-sidebar
        *ngIf="!selectedChatId && !receiverUserId"
        (selectedChatId)="onChatSelected($event)"
        (userSelected)="onUserSelected($event)">
      </app-chat-sidebar>

      <app-chat-area
        *ngIf="selectedChatId || receiverUserId"
        [selectedChatId]="selectedChatId"
        [receiverId]="receiverUserId">
      </app-chat-area>
    </div>
  `,
  styles: `
    :host {
      width: 100%;
      height: 100%;
    }
  `
})
export class ChatComponent implements OnInit {
  selectedChatId: string | null = null;
  receiverUserId: string | null = null;

  constructor(
    private chatService: ChatService,
    private cookieService: CookieService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.selectedChatId = params.get('chatId');
    });

    this.route.queryParamMap.subscribe(params => {
      this.receiverUserId = params.get('receiverId');
    });
  }

  onChatSelected(chatId: string) {
    this.receiverUserId = null;
    this.router.navigate(['/chats', chatId]);
  }

  onUserSelected(userId: string) {
    const currentUserId = this.cookieService.get("userId").response!;

    this.chatService.getAll().subscribe(result => {
      if (!result.isSuccess) return;

      const chats = result.response;
      const existingChat = chats.find(chat =>
        chat.userChats.some(uc => uc.userId === userId) &&
        chat.userChats.some(uc => uc.userId === currentUserId)
      );

      if (existingChat) {
        this.router.navigate(['/chats', existingChat.id]);
      } else {
        this.selectedChatId = null;
        this.receiverUserId = userId;
        this.router.navigate(['/chats'], { queryParams: { receiverId: userId } });
      }
    });
  }
}
