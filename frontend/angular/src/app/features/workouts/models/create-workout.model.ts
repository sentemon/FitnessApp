import {Level} from "./level.model";
import {CreateExercise} from "./create-exercise.model";

export interface CreateWorkout {
  title: string,
  description: string;
  durationInMinutes: number;
  level: Level;
  exercises: CreateExercise[]
}
