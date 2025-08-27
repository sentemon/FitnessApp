import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {Exercise} from "../../models/exercise.model";
import {ExerciseService} from "../../services/exercise.service";
import {Level} from "../../models/level.model";

@Component({
  selector: 'app-exercise',
  templateUrl: './exercise.component.html',
  styleUrl: './exercise.component.scss'
})
export class ExerciseComponent {
  @Input() workout!: Workout;

  @Output() workoutChange = new EventEmitter<Workout>();

  newExerciseName: string = '';

  constructor(private exerciseService: ExerciseService) { }

  addExercise(newExerciseName: string, level: Level): void {
    const tempId = "temp" + Date.now();
    const newExercise: Exercise = {
      id: tempId,
      name: newExerciseName,
      level: level,
      sets: []
    };

    this.exerciseService.add(newExerciseName).subscribe(result => {
      const workout: Workout = {
        ...this.workout,
        exercises: this.workout.exercises.concat(newExercise)
      };
      this.newExerciseName = '';

      const exercise = workout.exercises.find(e => e.id === tempId);
      exercise!.id = result.id

      this.workoutChange.emit(workout);
    });
  }

  deleteExercise(id: string): void {
    this.workout.exercises = this.workout.exercises.filter(e => e.id !== id);
    this.exerciseService.delete(id).subscribe();
  }

  protected updateWorkout(updatedExercise: Exercise) {
    this.workout = {
      ...this.workout,
      exercises: this.workout.exercises.map(e =>
        e.id === updatedExercise.id ? updatedExercise : e
      )
    };
  }

  protected readonly Level = Level;
}
