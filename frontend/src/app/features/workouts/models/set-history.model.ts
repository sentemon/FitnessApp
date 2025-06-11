export interface SetHistory {
  id: string;
  exerciseHistoryId: string;
  reps: number;
  weight: number;
  completed: boolean;
  completedAt: Date;
}
