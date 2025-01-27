import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";
import {SetService} from "../../services/set.service";

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrl: './workout.component.scss'
})
export class WorkoutComponent implements OnInit {
  workout!: Workout;

  constructor(
    private workoutService: WorkoutService,
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
    this.setService.markAsCompleted(set.id).subscribe(result => set.completed = !result);
  }

  isWorkoutCompleted(): boolean {
    return this.workout.exercises.every(exercise => exercise.sets.every(set => set.completed));
  }
}
