import { Injectable } from '@angular/core';
import {Observable, of} from "rxjs";
import {Comment} from "../models/comment.model";

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
        content: "string",
        createdAt: new Date()
      },
      {
        id: "string",
        postId: "string",
        userId: "string",
        content: "string",
        createdAt: new Date()
      },
    ]);
  }

}
