import {Component, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";
import {PostService} from "../../../posts/services/post.service";
import {Post} from "../../../posts/models/post.model";
import {AuthService} from "../../services/auth.service";
import {ActivatedRoute, Router} from "@angular/router";
import {UserDto} from "../../models/user-dto.model";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  user!: UserDto;
  currentUser!: User;

  posts: Post[] = [];
  selectedPostForModal: Post | null = null;

  followers: User[] | null = null;
  following: User[] | null = null;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private postService: PostService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const username = params.get("username")!;

      this.userService.getUserByUsername(username).subscribe(result => {
        if (result.isSuccess) {
          this.user = result.response;
        } else {
          console.warn(result.error.message);
        }
      });
    });


    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
        this.currentUser = result.response;
        this.getPosts();
      } else {
        console.warn(result.error.message);
      }
    })
  }

  logout(): void {
    this.authService.logout().subscribe(() => {
      this.router.navigate(["/"]).then(() => window.location.reload());
    });
  }

  private getPosts(): void {
    this.postService.getAllUserPosts(this.user.username).subscribe(result => {
      if (result.isSuccess) {
        this.posts = result.response;
      } else {
        console.warn(result.error.message);
      }
    })
  }

  openPost(post: Post): void {
    this.selectedPostForModal = post;
  }

  updatePost(updatedPost: Post) {
    const index = this.posts.findIndex(post => post.id === updatedPost.id);
    if (index !== -1) {
      this.posts[index] = updatedPost;
    }
  }

  openFollowers() {
    this.userService.getFollowers().subscribe(result => {
      if (!result.isSuccess) {
        console.log(result.error.message);
        return;
      }

      this.followers = result.response;
    });
  }

  openFollowing() {
    this.userService.getFollowing().subscribe(result => {
      if (!result.isSuccess) {
        console.log(result.error.message);
        return;
      }

      this.following = result.response;
    });
  }
}
