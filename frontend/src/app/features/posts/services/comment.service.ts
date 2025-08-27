import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import {Comment} from "../models/comment.model";
import {Apollo, ApolloBase} from "apollo-angular";
import {CREATE_COMMENT, DELETE_COMMENT, GET_ALL_COMMENTS} from "../graphql/mutations.graphql";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private postClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.postClient = apollo.use("posts");
  }

  public getAllComments(postId: string, first: number): Observable<Result<Comment[]>> {
    return this.postClient.mutate({
      mutation: GET_ALL_COMMENTS,
      variables: { postId, first }
    }).pipe(
      toResult<Comment[]>("allComments")
    );
  }

  addComment(postId: string, content: string): Observable<Result<Comment>> {
    return this.postClient.mutate({
      mutation: CREATE_COMMENT,
      variables: { postId, content }
    }).pipe(
      toResult<Comment>("createComment")
    );
  }

  deleteComment(id: string): Observable<Result<string>> {
    return this.postClient.mutate({
      mutation: DELETE_COMMENT,
      variables: { id }
    }).pipe(
      toResult<string>("deleteComment")
    );
  }
}
