import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {Exercise} from "../../models/exercise.model";
import {ExerciseService} from "../../services/exercise.service";

@Component({
  selector: 'app-exercise',
  templateUrl: './exercise.component.html',
  styleUrl: './exercise.component.scss'
})
export class ExerciseComponent {
  @Input() workout!: Workout;

  newExerciseName: string = '';

  constructor(private exerciseService: ExerciseService) { }

  addExercise(newExerciseName: string): void {
    const tempId = "temp" + Date.now();
    const newExercise: Exercise = {
      id: tempId,
      name: newExerciseName,
      sets: []
    };

    this.workout.exercises.push(newExercise);
    this.newExerciseName = '';

    const exercise = this.workout.exercises.find(e => e.id === tempId)!;
    this.exerciseService.add(newExerciseName).subscribe(result => exercise.id = result.id);
  }

  deleteExercise(id: string): void {
    this.workout.exercises = this.workout.exercises.filter(e => e.id !== id);
    this.exerciseService.delete(id).subscribe();
  }
}
