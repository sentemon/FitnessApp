import {UserChat} from "./user-chat";

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  username: string;
  imageUrl: string;
  email: string;
  isOnline: boolean;
  userChats: UserChat[];
}
