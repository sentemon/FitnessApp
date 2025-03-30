import {User} from "./user.model";
import {Message} from "./message.model";

export interface Chat {
  id: string;
  messages: Message[];
  users: User[];
}
