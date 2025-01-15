import {Component, OnInit} from '@angular/core';
import {Post} from "../../models/post.model";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {DatePipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {PostService} from "../../services/post.service";
import {LikeComponent} from "../like/like.component";
import {CommentComponent} from "../comment/comment.component";
import {PostModalComponent} from "../post-modal/post-modal.component";

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit{
  posts: Post[] = [];
  selectedPost: Post | null = null;

  protected readonly ContentType = ContentType;

  constructor(private postService: PostService) { }

  ngOnInit(): void {
    this.postService.getAllPosts(10, "4bb6a384-e0d7-43df-844f-efb017c02d7d").subscribe(posts => {
      this.posts = posts;
    });
  }

  openModal(post: Post): void {
    this.selectedPost = post;
  }

}
