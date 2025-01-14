import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {CommentComponent} from "./components/comment/comment.component";
import {LikeComponent} from "./components/like/like.component";
import {PostListComponent} from "./components/post-list/post-list.component";
import {PostModalComponent} from "./components/post-modal/post-modal.component";
import {FormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    CommentComponent,
    LikeComponent,
    PostListComponent,
    PostModalComponent
  ],
  exports: [
    PostListComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ]
})
export class PostsModule { }
