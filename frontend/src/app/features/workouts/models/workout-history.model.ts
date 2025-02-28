export interface WorkoutHistory {
  id: string;
  durationInMinutes: number;
  workoutId: string;
  userId: string;
  performedAt: Date;
  // exerciseHistories: ExerciseHistory[];
}
