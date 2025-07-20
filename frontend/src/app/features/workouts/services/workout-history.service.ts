import { Injectable } from '@angular/core';
import { Apollo, ApolloBase } from "apollo-angular";
import { WorkoutHistory } from "../models/workout-history.model";
import { Observable, of } from "rxjs";
import { ADD_WORKOUT_HISTORY } from "../graphql/mutations.graphql";
import { GET_ALL_WORKOUT_HISTORIES } from "../graphql/queries.graphql";
import { toResult } from "../../../core/extensions/graphql-result-wrapper";
import { Result } from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class WorkoutHistoryService {
  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts");
  }

  public add(workoutId: string): Observable<Result<WorkoutHistory>> {
    return this.workoutClient.mutate({
      mutation: ADD_WORKOUT_HISTORY,
      variables: { workoutId }
    }).pipe(
      toResult<WorkoutHistory>("addWorkoutHistory")
    );
  }

  public get(id: string): Observable<WorkoutHistory> {
    return of();
  }

  public getAll(): Observable<Result<WorkoutHistory[]>> {
    return this.workoutClient.query({
      query: GET_ALL_WORKOUT_HISTORIES
    }).pipe(
      toResult<WorkoutHistory[]>("allWorkoutHistories")
    );
  }
}
