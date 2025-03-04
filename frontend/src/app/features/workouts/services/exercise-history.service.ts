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
      setHistories: []
      // name: name,
      // level: Level.AllLevels,
    }

    return of(newExerciseHistory);
  }

  delete(id: string): Observable<string> {
    return of("");
  }
}
