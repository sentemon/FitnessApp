import {Chat} from "./chat.model";

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  isOnline: boolean;
  chats: Chat[];
}
