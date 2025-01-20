import {Component, OnInit} from '@angular/core';
import {Post} from "../../models/post.model";
import {PostService} from "../../services/post.service";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {UserService} from "../../../auth/services/user.service";

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit {
  posts: Post[] = [];
  selectedPostForModal: Post | null = null;
  selectedPostForOptions: Post | null = null;
  currentUsername!: string;

  protected readonly ContentType = ContentType;

  constructor(private postService: PostService, private userService: UserService) {

  }

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe(response => {
      this.posts = response
    });

    this.userService.getCurrentUser().subscribe(result => {
      this.currentUsername = result.username
    });
  }

  deletePost(postId: string) {
    this.postService.deletePost(postId).subscribe(result => {
      this.posts = this.posts.filter(post => post.id !== postId);
      console.log(result);
    });

    this.selectedPostForOptions = null;
  }

  openModal(post: Post): void {
    this.selectedPostForModal = post;
  }

  openOptions(post: Post): void {
    this.selectedPostForOptions = post;
  }
}
