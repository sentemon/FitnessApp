import {Injectable} from '@angular/core';
import {Exercise} from "../models/exercise.model";
import {Observable, of} from "rxjs";
import {Level} from "../models/level.model";

@Injectable({
  providedIn: 'root'
})
export class ExerciseService {
  constructor() { }

  add(name: string): Observable<Exercise> {
    const newExercise: Exercise = {
      level: Level.AllLevels,
      id: name,
      name: name,
      sets: []
    }

    return of(newExercise);
  }

  delete(id: string): Observable<string> {
    return of("");
  }
}
