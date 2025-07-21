import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Comment} from "../../models/comment.model";
import {CommentService} from "../../services/comment.service";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../../auth/services/user.service";

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.scss'
})
export class CommentsComponent implements OnInit {
  @ViewChild('scrollAnchor') scrollAnchor!: ElementRef;

  postId!: string;
  comments: Comment[] = [];
  newComment: string = "";

  currentUsername!: string;

  constructor(
    private route: ActivatedRoute,
    private commentService: CommentService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.postId = params.get("postId")!;

      this.commentService.getAllComments(this.postId, 10).subscribe(result => {
        if (result.isSuccess) {
          this.comments = result.response;
        } else {
          console.error(result.error.message);
        }
      })
    });

    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
        this.currentUsername = result.response.username.value;
      } else {
        console.error(result.error.message);
      }
    });
  }

  deleteComment(id: string) {
    this.commentService.deleteComment(id).subscribe(() => {
      this.comments = this.comments.filter(comment => comment.id !== id);
    });
  }

  sendComment(event: SubmitEvent, commentContent: string): void {
    event.preventDefault();

    if (commentContent.trim() === "")
      return;

    this.commentService.addComment(this.postId, commentContent).subscribe(result => {
      if (result.isSuccess) {
        this.comments.push(result.response);

        this.newComment = "";
      } else {
        console.log(result.error.message);
      }
    });

    setTimeout((): void => {
      this.scrollAnchor.nativeElement.scrollIntoView({ behavior: "smooth" });
    }, 0);
  }
}
