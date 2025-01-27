import { Injectable } from '@angular/core';
import {Exercise} from "../models/exercise.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ExerciseService {
  constructor() { }

  addExercise(name: string): Observable<Exercise> {
    const newExercise: Exercise = {
      id: name,
      name: name,
      sets: []
    }

    return of(newExercise);
  }
}
