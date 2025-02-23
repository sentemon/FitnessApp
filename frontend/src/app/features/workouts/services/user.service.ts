import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {Goal} from "../models/goal.model";
import {ActivityLevel} from "../models/activity-level.model";
import {WorkoutType} from "../models/workout-type.model";
import {MutationResponse} from "../graphql/mutation.response";
import {SET_UP_PROFILE} from "../graphql/mutations.graphql";
import {map, Observable} from "rxjs";
import {QueryResponse} from "../graphql/query.response";
import {PROFILE_SET_UP} from "../graphql/queries.graphql";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts");
  }

  public setUpProfile(weight: number, height: number, goal: Goal, activityLevel: ActivityLevel, dateOfBirth: string | null, favoriteWorkoutTypes: WorkoutType[]): Observable<string | undefined> {
    return this.workoutClient.mutate<MutationResponse>({
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
      map(response => response.data?.setUpProfile)
    );
  }

  public profileSetUp(): Observable<boolean> {
    return this.workoutClient.query<QueryResponse>({
      query: PROFILE_SET_UP
    }).pipe(
      map(response => response.data.profileSetUp)
    );
  }
}
