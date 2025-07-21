import {AfterViewInit, Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
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

  followers: User[] | null = null;
  following: User[] | null = null;
  isFollowing: boolean = false;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private postService: PostService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.getUser();
    this.getCurrentUser();
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

  updatePost(updatedPost: Post) {
    const index = this.posts.findIndex(post => post.id === updatedPost.id);
    if (index !== -1) {
      this.posts[index] = updatedPost;
    }
  }

  openFollowers() {
    this.userService.getFollowers(this.user.id).subscribe(result => {
      if (!result.isSuccess) {
        console.log(result.error.message);
        return;
      }

      this.followers = result.response;
    });
  }

  openFollowing() {
    this.userService.getFollowing(this.user.id).subscribe(result => {
      if (!result.isSuccess) {
        console.log(result.error.message);
        return;
      }

      this.following = result.response;
    });
  }

  follow(): void {
    this.userService.follow(this.user.id).subscribe(result => {
      if (result.isSuccess) {
        this.isFollowing = true;
      }
    });
  }

  unfollow(): void {
    this.userService.unfollow(this.user.id).subscribe(result => {
      if (result.isSuccess) {
        this.isFollowing = false;
      }
    });
  }

  private getUser(): void {
    this.route.paramMap.subscribe(params => {
      const username = params.get("username")!;

      this.userService.getUserByUsername(username).subscribe(result => {
        if (result.isSuccess) {
          this.user = result.response;

          this.getPosts();
          this.isUserFollowing();
        } else {
          console.log(result.error.message);
        }
      });
    });
  }

  private getCurrentUser(): void {
    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
        this.currentUser = result.response;
      } else {
        console.log(result.error.message);
      }
    });
  }

  private isUserFollowing(): void {
    this.userService.isFollowing(this.user.id).subscribe(result => {
      if (result.isSuccess) {
        this.isFollowing = result.response;
      }
    });
  }

  protected readonly console = console;
}
