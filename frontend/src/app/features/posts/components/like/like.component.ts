import {Component, Input} from '@angular/core';
import {Post} from "../../models/post.model";

@Component({
  selector: 'app-like',
  standalone: true,
  imports: [],
  templateUrl: './like.component.html',
  styleUrl: './like.component.scss'
})
export class LikeComponent {
  @Input() post!: Post;
}
