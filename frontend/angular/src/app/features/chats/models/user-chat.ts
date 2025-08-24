import {User} from "./user.model";
import {Chat} from "./chat.model";

export interface UserChat {
  userId: string;
  user: User;
  chatId: string;
  chat: Chat;
}
