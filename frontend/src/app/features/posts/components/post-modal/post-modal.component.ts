import {Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Post} from "../../models/post.model";
import {Comment} from "../../models/comment.model";
import {CommentService} from "../../services/comment.service";

@Component({
  selector: 'app-post-modal',
  templateUrl: './post-modal.component.html',
  styleUrl: './post-modal.component.scss'
})
export class PostModalComponent implements OnInit {
  @ViewChild('commentsContainer') commentsContainer!: ElementRef;
  @ViewChild('scrollAnchor') scrollAnchor!: ElementRef;

  @Input() post!: Post;
  @Output() close = new EventEmitter<void>();

  comments: Comment[] = [];
  newComment: string = "";

  constructor(private commentService: CommentService) { }

  ngOnInit(): void {
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

    this.commentService.addComment(this.post.id, commentContent).subscribe(comment =>{
      this.comments.push(comment);
      this.newComment = "";
    });

    setTimeout((): void => {
      this.scrollAnchor.nativeElement.scrollIntoView({ behavior: "smooth" })
    }, 0);
  }
}
