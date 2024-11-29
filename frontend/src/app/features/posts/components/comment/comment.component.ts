import {Component, Input} from '@angular/core';
import {Post} from "../../models/post.model";

@Component({
  selector: 'app-comment',
  standalone: true,
  imports: [],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.scss'
})
export class CommentComponent {
  @Input() post!: Post;
}
