import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Post} from "../../models/post.model";
import {LikeService} from "../../services/like.service";

@Component({
  selector: 'app-like',
  templateUrl: './like.component.html',
  styleUrl: './like.component.scss'
})
export class LikeComponent implements OnInit {
  @Input() post!: Post;
  @Output() likeChange = new EventEmitter<Post>();

  isLiked: boolean = false;

  constructor(private likeService: LikeService) { }

  ngOnInit(): void {
    this.likeService.isPostLiked(this.post.id).subscribe(isLiked => {
      this.isLiked = isLiked;
    });
  }

  toggleLike(): void {
    if (!this.isLiked) {
      this.likeService.addLike(this.post.id).subscribe(result => {
        if (result.isSuccess) {
          this.isLiked = true;
          const updatedPost = {...this.post, likeCount: this.post.likeCount + 1}
          this.likeChange.emit(updatedPost);
        } else {
          console.error(result.error.message);
        }
      });
    } else {
      this.likeService.deleteLike(this.post.id).subscribe(result => {
        if (result.isSuccess) {
          this.isLiked = false;
          const updatedPost = {...this.post, likeCount: this.post.likeCount - 1}
          this.likeChange.emit(updatedPost);
        } else {
          console.error(result.error.message);
        }
      });
    }
  }
}
