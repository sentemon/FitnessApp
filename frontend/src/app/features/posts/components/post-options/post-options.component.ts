import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Post} from "../../models/post.model";
import {PostService} from "../../services/post.service";

@Component({
  selector: 'app-post-options',
  templateUrl: './post-options.component.html',
  styleUrl: './post-options.component.scss'
})
export class PostOptionsComponent {
  @Input() post!: Post;
  @Output() close = new EventEmitter<void>();
  @Output() delete = new EventEmitter<string>();

  constructor(private postService: PostService) { }

  closeModal(): void {
    this.close.emit();
  }

  deletePost() {
    this.delete.emit(this.post.id);
  }
}
