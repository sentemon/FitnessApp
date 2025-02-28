import {Workout} from "../models/workout.model";
import {WorkoutHistory} from "../models/workout-history.model";

export interface MutationResponse {
  setUpProfile: string;
  createWorkout: Workout;
  addWorkoutHistory: WorkoutHistory;
}
