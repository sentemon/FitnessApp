import {Chat} from "./chat.model";
import {User} from "./user.model";

export interface Message {
  id: string;
  content: string;
  createdAt: Date;
  // updatedAt: Date;
  chatId: string;
  // chat: Chat;
  userId: string;
  // user: User;
}
