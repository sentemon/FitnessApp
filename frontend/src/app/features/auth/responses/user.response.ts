import {User} from "../models/user.model";

export interface UserResponse {
  userByUsername: User;
}
