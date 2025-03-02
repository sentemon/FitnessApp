import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {Exercise} from "../../models/exercise.model";
import {Set} from "../../models/set.model";
import {SetHistoryService} from "../../services/set-history.service";

@Component({
  selector: 'app-set-history',
  templateUrl: './set-history.component.html',
  styleUrl: './set-history.component.scss'
})
export class SetHistoryComponent {
  @Input() workout!: Workout;
  @Input() exercise!: Exercise;

  @Output() exerciseChange = new EventEmitter<Exercise>();

  newSet: Set = {
    id: '',
    reps: 0,
    weight: 0,
    completed: false,
    exerciseId: ''
  }

  constructor(private setHistoryService: SetHistoryService) { }

  markSetAsCompleted(setIndex: number): void {
    const set = this.exercise.sets[setIndex];

    this.setHistoryService.markAsCompleted(set.id).subscribe(result => {
      const updatedSets = this.exercise.sets.map((s, index) =>
        index === setIndex ? { ...s, completed: result } : s
      );

      const updatedExercise = { ...this.exercise, sets: updatedSets };

      this.exerciseChange.emit(updatedExercise);
    });
  }

  markSetAsUncompleted(setIndex: number): void {
    const set = this.exercise.sets[setIndex];

    this.setHistoryService.markAsUncompleted(set.id).subscribe(result => {
      const updatedSets = this.exercise.sets.map((s, index) =>
        index === setIndex ? { ...s, completed: !result } : s
      );

      const updatedExercise = { ...this.exercise, sets: updatedSets };

      this.exerciseChange.emit(updatedExercise);
    });
  }

  addSet(exerciseId: string, reps: number, weight: number): void {
    const tempId = "temp" + Date.now();
    const newSet: Set = {
      id: tempId,
      reps,
      weight,
      completed: false,
      exerciseId
    };

    const updatedSets = [...this.exercise.sets, newSet];
    const updatedExercise = { ...this.exercise, sets: updatedSets };

    this.exerciseChange.emit(updatedExercise);

    this.setHistoryService.add(exerciseId, reps, weight).subscribe(response => {
      const finalSets = updatedExercise.sets.map(s =>
        s.id === tempId ? { ...s, id: response.id } : s
      );

      this.exerciseChange.emit({ ...updatedExercise, sets: finalSets });
    });
  }

  deleteSet(setId: string): void {
    const updatedSets = this.exercise.sets.filter(s => s.id !== setId);
    const updatedExercise = { ...this.exercise, sets: updatedSets };

    this.exerciseChange.emit(updatedExercise);

    this.setHistoryService.delete(setId).subscribe();
  }
}
