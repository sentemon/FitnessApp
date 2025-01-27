import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";
import {SetService} from "../../services/set.service";
import {Set} from "../../models/set.model";
import {ExerciseService} from "../../services/exercise.service";
import {Exercise} from "../../models/exercise.model";

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrl: './workout.component.scss'
})
export class WorkoutComponent implements OnInit {
  workout!: Workout;
  newExerciseName: string = '';
  newSet: Set = {
    id: '',
    reps: 0,
    weight: 0,
    completed: false,
    exerciseId: ''
  }

  constructor(
    private workoutService: WorkoutService,
    private exerciseService: ExerciseService,
    private setService: SetService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const workoutUrl = params["workout-name"];
      this.workoutService.getWorkoutByUrl(workoutUrl).subscribe(result => {
        if (result) {
          this.workout = result
        } else {
          this.router.navigate(["/not-found"]);
        }
      })
    });
  }

  markSetAsCompleted(exerciseIndex: number, setIndex: number): void {
    const set = this.workout.exercises[exerciseIndex].sets[setIndex];
    this.setService.markAsCompleted(set.id).subscribe(result => set.completed = result);
  }

  markSetAsUncompleted(exerciseIndex: number, setIndex: number): void {
    const set = this.workout.exercises[exerciseIndex].sets[setIndex];
    this.setService.markAsUncompleted(set.id).subscribe(result => set.completed = !result);
  }

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

  addSet(exerciseId: string, reps: number, weight: number): void {
    const tempId = "temp" + Date.now();
    const newSet: Set = {
      id: tempId,
      reps: reps,
      weight: weight,
      completed: false,
      exerciseId: exerciseId
    };

    const exercise = this.workout.exercises.find(e => e.id === exerciseId);
    if (!exercise) {
      console.error(`Exercise with ID ${exerciseId} not found.`);
      return;
    }

    exercise.sets.push(newSet);
    this.newSet = { completed: false, exerciseId: "", id: "", reps: 0, weight: 0 }

    this.setService.add(exerciseId, reps, weight).subscribe(response => {
      const set = exercise.sets.find(s => s.id === tempId);
      if (set) {
        set.id = response.id;
      } else {
        console.error(`Temporary set with ID ${tempId} not found.`);
      }
    });
  }

  deleteSet(setId: string): void {
    const exercise = this.workout.exercises.find(e => e.sets.some(s => s.id === setId));

    if (exercise) {
      exercise.sets = exercise.sets.filter(s => s.id !== setId);
    } else {
      console.error(`Set with ID ${setId} not found.`);
    }

    this.setService.delete(setId).subscribe();
  }


  isWorkoutCompleted(): boolean {
    return this.workout.exercises.length > 0 &&
      this.workout.exercises.every(exercise =>
        exercise.sets.length > 0 &&
        exercise.sets.every(set => set.completed)
      );
  }
}
