import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Post} from "../../models/post.model";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {PostService} from "../../services/post.service";

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {
  posts: Post[] = [];
  selectedPost: Post | null = null;

  protected readonly ContentType = ContentType;

  constructor(private postService: PostService, private cdRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe(response => {
      this.posts = response
    });
  }

  openModal(post: Post): void {
    this.selectedPost = post;
  }
}
