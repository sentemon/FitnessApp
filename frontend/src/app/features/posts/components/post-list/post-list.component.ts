import {Component, OnInit} from '@angular/core';
import {Post} from "../../models/post.model";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {DatePipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {PostService} from "../../services/post.service";

@Component({
  selector: 'app-post-list',
  standalone: true,
  imports: [
    DatePipe,
    NgIf,
    NgForOf,
    NgOptimizedImage
  ],
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.scss'
})
export class PostListComponent implements OnInit{
  constructor(private postService: PostService) { }

  protected readonly ContentType = ContentType;

  posts: Post[] = [];

  ngOnInit(): void {
    this.postService.getAllPosts().subscribe(posts => {
      this.posts = posts;
    });
  }
  
}
