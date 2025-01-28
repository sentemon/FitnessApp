import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Exercise} from "../../models/exercise.model";
import {Set} from "../../models/set.model";
import {SetService} from "../../services/set.service";
import {Workout} from "../../models/workout.model";

@Component({
  selector: 'app-set',
  templateUrl: './set.component.html',
  styleUrl: './set.component.scss'
})
export class SetComponent {
  @Input() workout!: Workout;
  @Input() exercise!: Exercise;

  newSet: Set = {
    id: '',
    reps: 0,
    weight: 0,
    completed: false,
    exerciseId: ''
  }

  constructor(private setService: SetService) { }

  markSetAsCompleted(setIndex: number): void {
    const set = this.exercise.sets[setIndex];
    this.setService.markAsCompleted(set.id).subscribe(result => set.completed = result);
  }

  markSetAsUncompleted(setIndex: number): void {
    const set = this.exercise.sets[setIndex];
    this.setService.markAsUncompleted(set.id).subscribe(result => set.completed = !result);
  }

  addSet(exerciseId: string, reps: number, weight: number): void {
    const tempId = "temp" + Date.now();
    const newSet: Set = {
      id: tempId,
      reps: reps,
      weight: weight,
      completed: false,
      exerciseId: exerciseId
    };


    this.exercise.sets.push(newSet);
    this.newSet = { completed: false, exerciseId: "", id: "", reps: 0, weight: 0 }

    this.setService.add(exerciseId, reps, weight).subscribe(response => {
      const set = this.exercise.sets.find(s => s.id === tempId);
      if (set) {
        set.id = response.id;
      } else {
        console.error(`Temporary set with ID ${tempId} not found.`);
      }
    });
  }

  deleteSet(setId: string): void {
    this.exercise.sets = this.exercise.sets.filter(s => s.id !== setId);

    this.setService.delete(setId).subscribe();
  }
}
