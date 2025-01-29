import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrl: './workout.component.scss'
})
export class WorkoutComponent implements OnInit {
  workout!: Workout

  constructor(
    private workoutService: WorkoutService,
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

  isWorkoutCompleted(): boolean {
    return this.workout.exercises.length > 0 &&
      this.workout.exercises.every(exercise =>
        exercise.sets.length > 0 &&
        exercise.sets.every(set => set.completed)
      );
  }
}
