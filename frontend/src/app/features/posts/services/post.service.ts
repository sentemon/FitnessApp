import {Injectable} from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {BehaviorSubject, filter, map, Observable, tap} from "rxjs";
import {Post} from "../models/post.model";
import {UpdatePostDto} from "../requests/update-post.dto";
import {ContentType} from "../../../core/enums/content-type.enum";
import {GET_ALL_POSTS, GET_POST} from "../graphql/queries.graphql";
import {ApolloQueryResult} from "@apollo/client";
import {QueryResponse} from "../graphql/query.response";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {CREATE_POST, DELETE_POST} from "../graphql/mutations.graphql";
import {environment} from "../../../../environments/environment";
import {MutationResponse} from "../graphql/mutation.response";
import {Result} from "../../../core/types/result/result.type";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private postClient: ApolloBase;

  private postsSubject = new BehaviorSubject<Post[]>([]);
  private posts$ = this.postsSubject.asObservable();

  constructor(apollo: Apollo, private http: HttpClient) {
    this.postClient = apollo.use("posts");

    let first = 10;
    let lastPostId = "4bb6a384-e0d7-43df-844f-efb017c02d7d";

    let existingPosts: Post[] = [
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
        userImageUrl: "assets/profile.svg"
      },
      {
        commentCount: 34,
        contentType: ContentType.Image,
        contentUrl: "https://images.pexels.com/photos/863988/pexels-photo-863988.jpeg", // No Copyright
        createdAt: new Date(2024, 10, 20),
        description: "I love swimming!",
        id: "215e4b41-b712-46e2-87a1-9fec01899c7d",
        likeCount: 193,
        title: "",
        username: "example",
        userImageUrl: "assets/profile.svg"
      },
      {
        commentCount: 91,
        contentType: ContentType.Image,
        contentUrl: "https://images.pexels.com/photos/248547/pexels-photo-248547.jpeg", // No Copyright
        createdAt: new Date(2024, 10, 19),
        description: "Yesterday's challenge was great",
        id: "215e4b41-b712-46e2-87a1-9fec01899c7d",
        likeCount: 393,
        title: "Title",
        username: "example",
        userImageUrl: "http://localhost:8000/file/files/1d7ce09a-e88b-424f-854c-516b739426dd"
      }
    ];

    this.postClient.query<QueryResponse>({
      query: GET_ALL_POSTS,
      variables: { first, lastPostId }
    }).pipe(
      tap(response => {
        let posts = existingPosts.concat(response.data.allPost);

        posts.forEach(post => this.addPost(post));
      })
    ).subscribe();
  }

  getAllPosts(): Observable<Result<Post[]>> {
    return this.posts$.pipe(
      map(value => Result.success(value))
    );
  }

  getAllUserPosts(username: string): Observable<Result<Post[]>> {
    return this.posts$.pipe(
      map(posts => {
        const filteredPosts = posts.filter(p => p.username === username);
        return Result.success(filteredPosts);
      })
    );
  }

  getPost(id: string): Observable<Result<Post>> {
    return this.postClient.query<QueryResponse>({
      query: GET_POST,
      variables: { id }
    }).pipe(
      toResult<Post>("post")
    );
  }

  createPost(title: string, description: string, contentType: ContentType, contentFile: File): Observable<any> {
    const formData = new FormData();

    const operations = JSON.stringify({
      query: CREATE_POST,
      variables: {
        title: title,
        description: description,
        contentType: contentType,
        file: null
      },
    });

    const map = JSON.stringify({
      '0': ['variables.file']
    });

    formData.append("operations", operations);
    formData.append("map", map);

    formData.append('0', contentFile);

    return this.http.post(environment.post_service, formData, {
      headers: new HttpHeaders().set('GraphQL-Preflight', 'true')
    });
  }

  addPost(newPost: Post): void {
    this.postsSubject.next([newPost, ...this.postsSubject.value]);
  }

  updatePost(updatePost: UpdatePostDto): Observable<Post> {
    throw new Error();
  }

  deletePost(id: string): Observable<Result<string>> {
    return this.postClient.mutate<MutationResponse>({
      mutation: DELETE_POST,
      variables: { id }
    }).pipe(
      toResult<string>("deletePost")
    );
  }
}
