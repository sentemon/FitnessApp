import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Post} from "../../models/post.model";
import {DatePipe, NgForOf, NgIf} from "@angular/common";
import {Comment} from "../../models/comment.model";
import {CommentService} from "../../services/comment.service";
import {FormsModule} from "@angular/forms";
import {CreateCommentDto} from "../../requests/create-comment.dto";

@Component({
  selector: 'app-post-modal',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    DatePipe,
    FormsModule
  ],
  templateUrl: './post-modal.component.html',
  styleUrl: './post-modal.component.scss'
})
export class PostModalComponent implements OnInit {
  @Input() post!: Post;
  @Output() close = new EventEmitter<void>();

  comments: Comment[] = [];
  newComment: string = "";

  constructor(private commentService: CommentService) { }

  ngOnInit() {
    this.commentService.getAllComments(this.post.id).subscribe(comments => {
      this.comments = comments;
    });
  }

  closeModal(): void {
    this.close.emit();
  }

  sendComment(event: SubmitEvent, commentContent: string) {
    event.preventDefault();

    if (commentContent.trim() === "")
      return;

    const newCommentDto: CreateCommentDto = {
      postId: this.post.id,
      content: commentContent
    }

    this.commentService.addComment(newCommentDto).subscribe(comment =>{
      this.comments.push(comment);
      this.newComment = "";
    });
  }
}
