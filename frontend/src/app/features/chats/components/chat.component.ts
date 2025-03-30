import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-chat',
  template: `
    <div class="chat">
      <app-chat-sidebar (selectedChatId)="onChatSelected($event)"></app-chat-sidebar>
      <app-chat-area [selectedChatId]="selectedChatId"></app-chat-area>
    </div>
  `,
  styles: `
    :host {
      width: 100%;
      height: 100%;
    }

    .chat {
      display: flex;
    }
  `
})
export class ChatComponent implements OnInit {
  selectedChatId: string | null = null;

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.selectedChatId = params.get('chatId');
    });
  }

  onChatSelected(chatId: string) {
    this.router.navigate(['/chats', chatId]);
  }
}
