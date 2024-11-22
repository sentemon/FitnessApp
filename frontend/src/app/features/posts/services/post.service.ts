import { Injectable } from '@angular/core';
import {Apollo} from "apollo-angular";
import {CreatePostDto} from "../requests/create-post.dto";
import {Observable} from "rxjs";
import {Post} from "../models/post.model";
import {UpdatePostDto} from "../requests/update-post.dto";

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private apollo: Apollo) { }

  getPost(id: string): Observable<Post> {
    throw new Error();
  }

  getAllPosts(): Observable<Post[]> {
    throw new Error();
  }

  createPost(createPost: CreatePostDto): Observable<Post> {
    throw new Error();
  }

  updatePost(updatePost: UpdatePostDto): Observable<Post> {
    throw new Error();
  }

  deletePost(id: string): string {
    throw new Error();
  }
}
