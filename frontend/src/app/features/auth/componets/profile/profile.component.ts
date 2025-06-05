import {Component, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";
import {PostService} from "../../../posts/services/post.service";
import {Post} from "../../../posts/models/post.model";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  user!: User;
  posts: Post[] = [];

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private postService: PostService
  ) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
        this.user = result.response;
        this.getPosts();
      } else {
        console.warn(result.error.message);
      }
    })
  }

  logout(): void {
    this.authService.logout().subscribe(result => {
      console.log(result);
    })
  }
  private getPosts() {
    this.postService.getAllPosts().subscribe(result => {
      if (result.isSuccess) {
        this.posts = result.response;
      } else {
        console.warn(result.error.message);
      }
    })
  }
}
