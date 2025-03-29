import {User} from "./user.model";
import {UserChat} from "./user-chat.model";

export interface Chat {
  id: string;
  userChats: UserChat[];

}
