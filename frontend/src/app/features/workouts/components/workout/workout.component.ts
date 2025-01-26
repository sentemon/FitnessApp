import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Workout} from "../../models/workout.model";
import {Set} from "../../models/set.model";
import {WorkoutService} from "../../services/workout.service";

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrl: './workout.component.scss'
})
export class WorkoutComponent implements OnInit {
  workout!: Workout;
  newSet: Set = { reps: 0, weight: 0 };
  // newExerciseName = '';

  constructor(private workoutService: WorkoutService, private route: ActivatedRoute, private router: Router) { }

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

  addSet(exerciseIndex: number): void {
    const exercise = this.workout.exercises[exerciseIndex];
    if (this.newSet.reps > 0) {
      exercise.sets.push({ ...this.newSet });
      this.newSet = { reps: 0, weight: 0 };
    }
  }

  // addExercise(): void {
  //   if (this.newExerciseName.trim()) {
  //     this.workout.exercises.push({
  //       id: ,
  //       name: this.newExerciseName,
  //       sets: [],
  //     });
  //     this.newExerciseName = '';
  //   }
  // }
}
