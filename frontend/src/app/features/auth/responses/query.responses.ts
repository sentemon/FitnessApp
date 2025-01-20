import { User } from "../models/user.model";

export interface QueryResponses {
  currentUser: User
  userByUsername: User
  isAuthenticated: boolean
}
