import {Component} from '@angular/core';
import {Post} from "../../models/post.model";
import {ContentType} from "../../../../core/enums/content-type.enum";
import {DatePipe, NgForOf, NgIf, NgOptimizedImage} from "@angular/common";

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
export class PostListComponent {
  protected readonly ContentType = ContentType;

  // Mocks
  posts: Post[] = [
    {
      commentCount: 4,
      contentType: ContentType.Image,
      contentUrl: "https://www.istockphoto.com/resources/images/PhotoFTLP/P5-NOV-iStock-2158268393.jpg", // No Copyright
      createdAt: new Date(),
      description: "the men play football",
      id: "215e4b41-b712-46e2-87a1-9fec01899c7d",
      likeCount: 56,
      title: "Title",
      username: "example",
      imageUrl: "assets/profile.svg"
    }
  ];
}
