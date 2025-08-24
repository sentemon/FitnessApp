import {Level} from "./level.model";
import {CreateSet} from "./create-set.model";

export interface CreateExercise {
  name: string;
  level: Level;
  sets: CreateSet[]
}
