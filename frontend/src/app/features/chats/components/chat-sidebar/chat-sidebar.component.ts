import { Component } from '@angular/core';
import {Chat} from "../../models/chat.model";

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrl: './chat-sidebar.component.scss'
})
export class ChatSidebarComponent {
  chats: Chat[] = [];
}
