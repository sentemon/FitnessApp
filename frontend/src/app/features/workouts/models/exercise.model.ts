import {Set} from "./set.model";
import {Level} from "./level.model";

export interface Exercise {
  id: string;
  name: string;
  level: Level;
  sets: Set[];
  // userId: string;
}
