import { Injectable } from '@angular/core';
import {Apollo, ApolloBase} from "apollo-angular";
import {Goal} from "../models/goal.model";
import {ActivityLevel} from "../models/activity-level.model";
import {WorkoutType} from "../models/workout-type.model";
import {MutationResponse} from "../graphql/mutation.response";
import {SET_UP_PROFILE} from "../graphql/mutations.graphql";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts");
  }

  public setUpProfile(weight: number, height: number, goal: Goal, activityLevel: ActivityLevel, dateOfBirth: Date, favoriteWorkoutTypes: WorkoutType[]): Observable<string | undefined> {
    return this.workoutClient.mutate<MutationResponse>({
      mutation: SET_UP_PROFILE,
      variables: { weight, height, goal, activityLevel, dateOfBirth, favoriteWorkoutTypes }
    }).pipe(
      map(response => response.data?.setUpProfile)
    );
  }
}
