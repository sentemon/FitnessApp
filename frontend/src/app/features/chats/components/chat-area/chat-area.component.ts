import {Component, Input, OnChanges, OnInit} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";

@Component({
  selector: 'app-chat-area',
  templateUrl: './chat-area.component.html',
  styleUrl: './chat-area.component.scss'
})
export class ChatAreaComponent implements OnChanges {
  selectedChat: Chat | null = null;

  @Input() selectedChatId: string | null = null;

  constructor(private chatService: ChatService) { }

  ngOnChanges() {
    this.chatService.get(this.selectedChatId).subscribe(result => this.selectedChat = result);
  }
}
