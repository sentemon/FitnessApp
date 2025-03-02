import {Workout} from "./workout.model";

export interface WorkoutHistory {
  id: string;
  durationInMinutes: number;
  workout: Workout;
  userId: string;
  performedAt: Date;
  // exerciseHistories: ExerciseHistory[];
}
