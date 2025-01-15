import {Post} from "../models/post.model";

export interface QueryResponse {
  post: Post
  allPost: Post[]
}
