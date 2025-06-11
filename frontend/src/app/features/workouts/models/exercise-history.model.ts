import {SetHistory} from "./set-history.model";

export interface ExerciseHistory {
  id: string;
  workoutHistoryId: string;
  exerciseId: string;
  setHistories: SetHistory[];
}
