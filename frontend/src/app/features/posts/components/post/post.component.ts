import { Component, Input, OnInit } from '@angular/core';
import {ContentType} from "../../../../core/enums/content-type.enum";
import {Post} from "../../models/post.model";
import {PostService} from "../../services/post.service";
import {ActivatedRoute, Route, Router} from "@angular/router";
import {UserService} from "../../../auth/services/user.service";

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent implements OnInit {
  @Input() post!: Post;

  currentUsername!: string;
  postOptions: boolean = false;
  postModal: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private postService: PostService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const postId = params.get('postId')!;

      this.postService.getPost(postId).subscribe(result => {
        if (result.isSuccess) {
          this.post = result.response;
        } else {
          console.log(result.error.message);
        }
      });
    });

    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
       this.currentUsername = result.response.username.value;
      } else {
        console.log(result.error.message);
      }
    });
  }

  protected readonly ContentType = ContentType;

  updatePost(updatedPost: Post): void {
    this.post = updatedPost;
  }

  deletePost(): void {
    this.postService.deletePost(this.post.id).subscribe(result => {
      if (result.isSuccess) {
        this.router.navigate(['/', this.currentUsername]);
      } else {
        console.error(result.error.message);
      }
    });
  }

  openOptions(): void {
    this.postOptions = true;
  }

  openComments() {
    if (window.innerWidth < 1024) {
      this.router.navigate(['/posts', this.post.id, 'comments']);
    } else {
      this.postModal = true;
    }
  }
}
