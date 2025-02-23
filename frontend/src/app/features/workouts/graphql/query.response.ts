import {Workout} from "../models/workout.model";

export interface QueryResponse {
  profileSetUp: boolean;
  allWorkouts: Workout[];
}
