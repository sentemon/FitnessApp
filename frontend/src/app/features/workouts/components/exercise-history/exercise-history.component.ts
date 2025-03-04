import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {ExerciseService} from "../../services/exercise.service";
import {Exercise} from "../../models/exercise.model";
import {Level} from "../../models/level.model";
import {WorkoutHistory} from "../../models/workout-history.model";
import {ExerciseHistory} from "../../models/exercise-history.model";
import {ExerciseHistoryService} from "../../services/exercise-history.service";

@Component({
  selector: 'app-exercise-history',
  templateUrl: './exercise-history.component.html',
  styleUrl: './exercise-history.component.scss'
})
export class ExerciseHistoryComponent {
  @Input() workoutHistory!: WorkoutHistory;

  @Output() workoutHistoryChange = new EventEmitter<WorkoutHistory>();

  newExerciseHistoryName: string = '';

  constructor(private exerciseHistoryService: ExerciseHistoryService) { }

  addExercise(newExerciseHistoryName: string, level: Level): void {
    const tempId = "temp" + Date.now();
    const newExercise: ExerciseHistory = {
      id: tempId,
      setHistories: []
      // name: newExerciseHistoryName,
      // level: level,
    };

    this.exerciseHistoryService.add(newExerciseHistoryName).subscribe(result => {
      const workoutHistory: WorkoutHistory = {
        ...this.workoutHistory,
        exerciseHistories: this.workoutHistory.exerciseHistories.concat(newExercise)
      };
      this.newExerciseHistoryName = '';

      const exercise = workoutHistory.exerciseHistories.find(e => e.id === tempId);
      exercise!.id = result.id

      this.workoutHistoryChange.emit(workoutHistory);
    });
  }

  deleteExercise(id: string): void {
    this.workoutHistory.exerciseHistories = this.workoutHistory.exerciseHistories.filter(e => e.id !== id);
    this.exerciseHistoryService.delete(id).subscribe();
  }

  protected readonly Level = Level;

  protected updateWorkout(updatedExercise: ExerciseHistory) {
    this.workoutHistory = {
      ...this.workoutHistory,
      exerciseHistories: this.workoutHistory.exerciseHistories.map(e =>
        e.id === updatedExercise.id ? updatedExercise : e
      )
    };
  }
}
