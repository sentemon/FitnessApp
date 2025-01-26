import {Set} from "./set.model";

export interface Exercise {
  id: string;
  name: string;
  sets: Set[];
  // userId: string;
}
