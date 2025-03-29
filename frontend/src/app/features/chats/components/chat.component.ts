import { Component } from '@angular/core';

@Component({
  selector: 'app-chat',
  template: `
    <div class="chat">
      <app-chat-sidebar></app-chat-sidebar>
      <app-chat-area></app-chat-area>
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

}
