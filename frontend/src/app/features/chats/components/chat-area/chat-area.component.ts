import {AfterViewChecked, Component, ElementRef, Input, OnChanges, ViewChild} from '@angular/core';
import {Chat} from "../../models/chat.model";
import {ChatService} from "../../services/chat.service";

@Component({
  selector: 'app-chat-area',
  templateUrl: './chat-area.component.html',
  styleUrl: './chat-area.component.scss'
})
export class ChatAreaComponent implements OnChanges, AfterViewChecked {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;
  selectedChat: Chat | null = null;

  @Input() selectedChatId: string | null = null;
  me: string = "user1";

  constructor(private chatService: ChatService) { }

  ngOnChanges() {
    this.chatService.get(this.selectedChatId).subscribe(result => this.selectedChat = result);
  }

  ngAfterViewChecked() {
    if (!this.messagesContainer) return;
    this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
  }

  protected readonly navigator = navigator;
}
