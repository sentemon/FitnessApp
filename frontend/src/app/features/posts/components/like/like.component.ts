import {Component, Input} from '@angular/core';
import {Post} from "../../models/post.model";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-like',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './like.component.html',
  styleUrl: './like.component.scss'
})
export class LikeComponent {
  @Input() post!: Post;
  isLiked: boolean = false;

  toggleLike(): void {
    this.isLiked = !this.isLiked;

    if (this.isLiked) {
      this.post.likeCount++;
    } else {
      this.post.likeCount--;
    }
  }
}
