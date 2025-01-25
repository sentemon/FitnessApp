import {Level} from "./level.model";

export interface Workout {
  id: string;
  title: string;
  description: string;
  time: number;
  level: Level;
  imageUrl: string;
}
