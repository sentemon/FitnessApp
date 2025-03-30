import {Component, OnInit} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrl: './chat-sidebar.component.scss'
})
export class ChatSidebarComponent implements OnInit {
  chats: Chat[] = [];

  constructor(private chatService: ChatService) { }

  ngOnInit() {
    this.chatService.getAll().subscribe(result => this.chats = result);
  }
}
