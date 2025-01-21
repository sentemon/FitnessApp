import { Injectable } from '@angular/core';
import {map, Observable, of} from "rxjs";
import {Comment} from "../models/comment.model";
import {Apollo, ApolloBase} from "apollo-angular";
import {MutationResponse} from "../graphql/mutation.response";
import {CREATE_COMMENT} from "../graphql/mutations.graphql";

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private postClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.postClient = apollo.use("posts");
  }

  public getAllComments(postId: string): Observable<Comment[]> {
    return of([
      {
        id: "string",
        postId: "string",
        userId: "string",
        username: "username",
        content: "Wow, Let's go!",
        createdAt: new Date()
      },
      {
        id: "string",
        postId: "string",
        userId: "string",
        username: "username",
        content: "Comment",
        createdAt: new Date()
      },
    ]);
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
}
