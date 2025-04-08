import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {Like} from "../models/like.model";
import {map, Observable} from "rxjs";
import {MutationResponse} from "../graphql/mutation.response";
import {ADD_LIKE, DELETE_LIKE, IS_POST_LIKED} from "../graphql/mutations.graphql";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  private postClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.postClient = apollo.use("posts");
  }

  public addLike(postId: string): Observable<Result<Like>> {
    return this.postClient.mutate<MutationResponse>({
      mutation: ADD_LIKE,
      variables: { postId }
    }).pipe(
      toResult<Like>("addLike")
    );
  }

  public deleteLike(postId: string): Observable<Result<string>> {
    return this.postClient.mutate<MutationResponse>({
      mutation: DELETE_LIKE,
      variables: { postId }
    }).pipe(
      toResult<string>("deleteLike")
    );
  }

  isPostLiked(postId: string): Observable<boolean> {
    return this.postClient.mutate<MutationResponse>({
      mutation: IS_POST_LIKED,
      variables: { postId }
    }).pipe(
      map(response => {
        return response.data!.isPostLiked;
      })
    );
  }
}
