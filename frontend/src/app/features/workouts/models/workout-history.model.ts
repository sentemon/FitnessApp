import {Workout} from "./workout.model";
import {ExerciseHistory} from "./exercise-history.model";

export interface WorkoutHistory {
  id: string;
  durationInMinutes: number;
  workout: Workout;
  userId: string;
  performedAt: Date;
  exerciseHistories: ExerciseHistory[];
}
