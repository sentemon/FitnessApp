import {Component, EventEmitter, Input, Output} from '@angular/core';
import {SetHistoryService} from "../../services/set-history.service";
import {SetHistory} from "../../models/set-history.model";
import {WorkoutHistory} from "../../models/workout-history.model";
import {ExerciseHistory} from "../../models/exercise-history.model";

@Component({
  selector: 'app-set-history',
  templateUrl: './set-history.component.html',
  styleUrl: './set-history.component.scss'
})
export class SetHistoryComponent {
  @Input() workoutHistory!: WorkoutHistory;
  @Input() exerciseHistory!: ExerciseHistory;

  @Output() exerciseHistoryChange = new EventEmitter<ExerciseHistory>();

  newSetHistory: SetHistory = {
    id: '',
    reps: 0,
    weight: 0,
    exerciseHistoryId: '',
    completed: false,
    completedAt: new Date()
  }

  constructor(private setHistoryService: SetHistoryService) { }

  markSetAsCompleted(setHistoryIndex: number): void {
    const set = this.exerciseHistory.setHistories[setHistoryIndex];

    this.setHistoryService.markAsCompleted(set.id).subscribe(result => {
      const updatedSets = this.exerciseHistory.setHistories.map((s, index) =>
        index === setHistoryIndex ? { ...s, completed: result } : s
      );

      const updatedExercise = { ...this.exerciseHistory, setHistories: updatedSets };

      this.exerciseHistoryChange.emit(updatedExercise);
    });
  }

  markSetAsUncompleted(setIndex: number): void {
    const set = this.exerciseHistory.setHistories[setIndex];

    this.setHistoryService.markAsUncompleted(set.id).subscribe(result => {
      const updatedSets = this.exerciseHistory.setHistories.map((s, index) =>
        index === setIndex ? { ...s, completed: !result } : s
      );

      const updatedExercise = { ...this.exerciseHistory, setHistories: updatedSets };

      this.exerciseHistoryChange.emit(updatedExercise);
    });
  }

  addSet(exerciseHistoryId: string, reps: number, weight: number): void {
    const tempId = "temp" + Date.now();
    const newSet: SetHistory = {
      id: tempId,
      reps,
      weight,
      exerciseHistoryId,
      completed: false,
      completedAt: new Date()
    };

    const updatedSets = [...this.exerciseHistory.setHistories, newSet];
    const updatedExercise = { ...this.exerciseHistory, setHistories: updatedSets };

    this.exerciseHistoryChange.emit(updatedExercise);

    this.setHistoryService.add(exerciseHistoryId, reps, weight).subscribe(response => {
      const finalSets = updatedExercise.setHistories.map(s =>
        s.id === tempId ? { ...s, id: response.id } : s
      );

      this.exerciseHistoryChange.emit({ ...updatedExercise, setHistories: finalSets });
    });
  }

  deleteSet(setHistoryId: string): void {
    const updatedSets = this.exerciseHistory.setHistories.filter(s => s.id !== setHistoryId);
    const updatedExercise = { ...this.exerciseHistory, setHistories: updatedSets };

    this.exerciseHistoryChange.emit(updatedExercise);

    this.setHistoryService.delete(setHistoryId).subscribe();
  }
}
