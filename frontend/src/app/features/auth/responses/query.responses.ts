import { User } from "../models/user.model";

export interface QueryResponses {
  userByUsername: User
  isAuthenticated: boolean
}
