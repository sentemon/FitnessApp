import { NgModule } from '@angular/core';
import {ChatComponent} from "./components/chat.component";
import {ChatAreaComponent} from "./components/chat-area/chat-area.component";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {ChatSidebarComponent} from "./components/chat-sidebar/chat-sidebar.component";

@NgModule({
  declarations: [
    ChatComponent,
    ChatSidebarComponent,
    ChatAreaComponent
  ],
  imports: [
    NgClass,
    FormsModule,
    DatePipe,
    NgForOf,
    NgIf
  ]
})
export class ChatsModule { }
