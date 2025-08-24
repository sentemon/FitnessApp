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

  @Output() exerciseChange = new EventEmitter<Exercise>();

  newSet: Set = {
    id: '',
    reps: 0,
    weight: 0,
    completed: false,
    exerciseId: ''
  }

  constructor(private setService: SetService) { }

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

    this.setService.add(exerciseId, reps, weight).subscribe(response => {
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

    this.setService.delete(setId).subscribe();
  }
}
