import {Component, OnInit} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";
import {ActivatedRoute, Router} from "@angular/router";
import {WorkoutHistory} from "../../models/workout-history.model";
import {WorkoutHistoryService} from "../../services/workout-history.service";

@Component({
  selector: 'app-workout-history',
  templateUrl: './workout-history.component.html',
  styleUrl: './workout-history.component.scss'
})
export class WorkoutHistoryComponent implements OnInit {
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
          this.workout = {
            id: result.id,
            title: result.title,
            description: result.description,
            durationInMinutes: result.durationInMinutes,
            level: result.level,
            url: result.url,
            exercises: result.exercises,
            imageUrl: result.imageUrl,
            isCustom: result.isCustom,
            userId: result.userId
          }

          console.log(result);
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

  protected updateWorkout(updatedWorkout: Workout) {
    this.workout = updatedWorkout;
  }
}
