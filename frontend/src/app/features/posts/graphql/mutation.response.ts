import {Post} from "../models/post.model";
import {Like} from "../models/like.model";
import {Comment} from "../models/comment.model";

export interface MutationResponse {
  createPost: Post;
  deletePost: string;

  allComments: Comment[];
  createComment: Comment;

  addLike: Like;
  deleteLike: string;
  isPostLiked: boolean
}
