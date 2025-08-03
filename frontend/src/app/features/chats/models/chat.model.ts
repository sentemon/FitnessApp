import {User} from "./user.model";
import {Message} from "./message.model";
import {UserChat} from "./user-chat";

export interface Chat {
  id: string;
  messages: Message[];
  userChats: UserChat[];
  user: User;
  createdAt: Date;
}
