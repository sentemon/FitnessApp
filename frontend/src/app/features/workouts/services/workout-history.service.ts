import { Injectable } from '@angular/core';
import {Apollo, ApolloBase, MutationResult} from "apollo-angular";
import {WorkoutHistory} from "../models/workout-history.model";
import {map, Observable, of} from "rxjs";
import {MutationResponse} from "../graphql/mutation.response";
import {ADD_WORKOUT_HISTORY} from "../graphql/mutations.graphql";
import {GET_ALL_WORKOUT_HISTORIES} from "../graphql/queries.graphql";
import {QueryResponse} from "../graphql/query.response";

@Injectable({
  providedIn: 'root'
})
export class WorkoutHistoryService {
  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts");
  }

  public add(workoutId: string): Observable<WorkoutHistory> {
    return this.workoutClient.mutate<MutationResponse>({
      mutation: ADD_WORKOUT_HISTORY,
      variables: { workoutId }
    }).pipe(
      map(response => response.data!.addWorkoutHistory)
    );
  }

  public get(id: string): Observable<WorkoutHistory> {
    return of();
  }

  public getAll(): Observable<WorkoutHistory[]> {
    return this.workoutClient.query<QueryResponse>({
      query: GET_ALL_WORKOUT_HISTORIES
    }).pipe(
      map(response => response.data.allWorkoutHistories)
    );
  }
}
