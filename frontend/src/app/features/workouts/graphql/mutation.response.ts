import {Workout} from "../models/workout.model";

export interface MutationResponse {
  setUpProfile: string;
  createWorkout: Workout;
}
