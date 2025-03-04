import { Injectable } from '@angular/core';
import {Observable, of} from "rxjs";
import {ExerciseHistory} from "../models/exercise-history.model";

@Injectable({
  providedIn: 'root'
})
export class ExerciseHistoryService {

  constructor() { }

  add(name: string): Observable<ExerciseHistory> {
    const newExerciseHistory: ExerciseHistory = {
      id: name,
      exerciseId: "",
      workoutHistoryId: "",
      setHistories: []
    }

    return of(newExerciseHistory);
  }

  delete(id: string): Observable<string> {
    return of("");
  }
}
