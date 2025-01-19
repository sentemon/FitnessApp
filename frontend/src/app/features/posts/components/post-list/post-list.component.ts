import {Component, OnInit} from '@angular/core';
import {Post} from "../../models/post.model";
import {PostService} from "../../services/post.service";
import {ContentType} from "../../../../core/enums/content-type.enum";

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {
  posts: Post[] = [];
  selectedPost: Post | null = null;

  constructor(private postService: PostService) { }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe(response => {
      this.posts = response
    });
  }

  openModal(post: Post): void {
    this.selectedPost = post;
  }

  protected readonly ContentType = ContentType;
}
