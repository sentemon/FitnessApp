import {Level} from "./level.model";
import {Exercise} from "./exercise.model";

export interface Workout {
  id: string;
  title: string;
  description: string;
  time: number;
  level: Level;
  url: string;
  exercises: Exercise[];
  imageUrl: string;
}
