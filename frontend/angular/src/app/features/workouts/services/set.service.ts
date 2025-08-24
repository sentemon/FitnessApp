import { Injectable } from '@angular/core';
import {Observable, of} from "rxjs";
import {Set} from "../models/set.model";

@Injectable({
  providedIn: 'root'
})
export class SetService {
  constructor() { }

  add(exerciseId: string, reps: number, weight: number): Observable<Set> {
    const newSet: Set = {
      id: 'newId' + Date.now(),
      reps: reps,
      weight: weight,
      completed: false,
      exerciseId: exerciseId
    };

    return of(newSet);
  }

  delete(id: string): Observable<boolean> {
    return of(true);
  }
}
