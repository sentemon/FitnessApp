import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {Goal} from "../models/goal.model";
import {ActivityLevel} from "../models/activity-level.model";
import {WorkoutType} from "../models/workout-type.model";
import {SET_UP_PROFILE} from "../graphql/mutations.graphql";
import {map, Observable} from "rxjs";
import {PROFILE_SET_UP} from "../graphql/queries.graphql";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts");
  }

  public setUpProfile(weight: number, height: number, goal: Goal, activityLevel: ActivityLevel, dateOfBirth: string | null, favoriteWorkoutTypes: WorkoutType[]): Observable<Result<string>> {
    return this.workoutClient.mutate({
      mutation: SET_UP_PROFILE,
      variables: {
        weight,
        height,
        goal,
        activityLevel,
        dateOfBirth,
        favoriteWorkoutTypes
      }
    }).pipe(
      toResult("setUpProfile")
    );
  }

  public profileSetUp(): Observable<Result<boolean>> {
    return this.workoutClient.query({
      query: PROFILE_SET_UP
    }).pipe(
      toResult("profileSetUp")
    );
  }
}
