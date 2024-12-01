import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Post} from "../../models/post.model";
import {NgForOf, NgIf} from "@angular/common";
import {Comment} from "../../models/comment.model";
import {CommentService} from "../../services/comment.service";

@Component({
  selector: 'app-post-modal',
  standalone: true,
  imports: [
    NgIf,
    NgForOf
  ],
  templateUrl: './post-modal.component.html',
  styleUrl: './post-modal.component.scss'
})
export class PostModalComponent implements OnInit {
  @Input() post!: Post;
  @Output() close = new EventEmitter<void>();

  comments: Comment[] = [];

  constructor(private commentService: CommentService) { }

  ngOnInit() {
    this.commentService.getAllComments(this.post.id).subscribe(comments => {
      this.comments = comments;
    });
  }

  closeModal(): void {
    this.close.emit();
  }
}
