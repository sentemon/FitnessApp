import { Injectable } from '@angular/core';
import {Observable, of} from "rxjs";
import {Comment} from "../models/comment.model";
import {CreateCommentDto} from "../requests/create-comment.dto";

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor() { }

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

  addComment(createCommentDto: CreateCommentDto): Observable<Comment> {
    return of({
      id: "string",
      postId: "string",
      userId: "string",
      username: "username",
      content: createCommentDto.content,
      createdAt: new Date()
    });
  }
}
