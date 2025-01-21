import { Injectable } from '@angular/core';
import {map, Observable} from "rxjs";
import {Comment} from "../models/comment.model";
import {Apollo, ApolloBase} from "apollo-angular";
import {MutationResponse} from "../graphql/mutation.response";
import {CREATE_COMMENT, DELETE_COMMENT, GET_ALL_COMMENTS} from "../graphql/mutations.graphql";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private postClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.postClient = apollo.use("posts");
  }

  public getAllComments(postId: string, first: number): Observable<Comment[]> {
    return this.postClient.mutate<MutationResponse>({
      mutation: GET_ALL_COMMENTS,
      variables: { postId, first }
    }).pipe(
      map(response => {
        return response.data!.allComments;
      })
    )
  }

  addComment(postId: string, content: string): Observable<Comment> {
    return this.postClient.mutate<MutationResponse>({
      mutation: CREATE_COMMENT,
      variables: { postId, content }
    }).pipe(
      map(response => {
        return response.data!.createComment;
      })
    )
  }

  deleteComment(id: string): Observable<string> {
    return this.postClient.mutate<MutationResponse>({
      mutation: DELETE_COMMENT,
      variables: { id }
    }).pipe(
      map(response => {
        return response.data!.deleteComment;
      })
    )
  }
}
