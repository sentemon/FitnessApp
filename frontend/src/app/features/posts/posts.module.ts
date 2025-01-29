import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {CommentComponent} from "./components/comment/comment.component";
import {LikeComponent} from "./components/like/like.component";
import {PostListComponent} from "./components/post-list/post-list.component";
import {PostModalComponent} from "./components/post-modal/post-modal.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { CreatePostComponent } from './components/create-post/create-post.component';
import {SharedModule} from "../../shared/shared.module";
import {PostService} from "./services/post.service";
import { PostOptionsComponent } from './components/post-options/post-options.component';



@NgModule({
  declarations: [
    CommentComponent,
    LikeComponent,
    PostListComponent,
    PostModalComponent,
    CreatePostComponent,
    PostOptionsComponent,
  ],
  exports: [
    PostListComponent,
    CreatePostComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    NgOptimizedImage,
  ],
  providers: [
    PostService
  ]
})
export class PostsModule { }
