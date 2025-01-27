import { Injectable } from '@angular/core';
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SetService {
  constructor() { }

  markAsCompleted(id: string): Observable<boolean> {
    return of(true);
  }

  markAsUncompleted(id: string): Observable<boolean> {
    return of(true);
  }
}
