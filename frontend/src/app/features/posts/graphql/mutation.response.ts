import {Post} from "../models/post.model";
import {Like} from "../models/like.model";

export interface MutationResponse {
  createPost: Post;
  deletePost: string;

  addLike: Like;
  deleteLike: string;
  isPostLiked: boolean
}
