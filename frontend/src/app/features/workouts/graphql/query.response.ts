import {Workout} from "../models/workout.model";
import {WorkoutHistory} from "../models/workout-history.model";

export interface QueryResponse {
  profileSetUp: boolean;
  allWorkouts: Workout[];
  allWorkoutHistories: WorkoutHistory[];
  workoutByUrl: Workout;
}
