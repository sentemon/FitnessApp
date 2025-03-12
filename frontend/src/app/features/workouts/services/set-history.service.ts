import { Injectable } from '@angular/core';
import {Observable, of} from "rxjs";
import {Set} from "../models/set.model";
import {Apollo, ApolloBase} from "apollo-angular";

@Injectable({
  providedIn: 'root'
})
export class SetHistoryService {
  private readonly workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts");
  }

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

  markAsCompleted(id: string): Observable<boolean> {
    return of(true);
  }

  markAsUncompleted(id: string): Observable<boolean> {
    return of(true);
  }
}
