import {User} from "./user.model";

export interface Message {
  id: string;
  senderId: string;
  sender: User;
  chatId: string;
  content: string;
  sentAt: Date;
  updatedAt?: Date;
}
