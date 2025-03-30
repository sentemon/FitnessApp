import { Component } from '@angular/core';

@Component({
  selector: 'app-chat',
  template: `
    <div class="chat">
      <app-chat-sidebar (chatSelected)="onChatSelected($event)"></app-chat-sidebar>
      <app-chat-area> [selectedChatId]="selectedChatId"</app-chat-area>
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
export class ChatComponent {
  selectedChatId: string | null = null;

  onChatSelected(chatId: string) {
    this.selectedChatId = chatId;
  }
}
