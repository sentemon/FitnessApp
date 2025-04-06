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
        if (result.isSuccess) {
          const workout = result.response;
          
          this.workout = {
            id: workout.id,
            title: workout.title,
            description: workout.description,
            durationInMinutes: workout.durationInMinutes,
            level: workout.level,
            url: workout.url,
            exercises: workout.exercises,
            imageUrl: workout.imageUrl,
            isCustom: workout.isCustom,
            userId: workout.userId
          }

          console.log(result);
        } else {
          this.router.navigate(["/not-found"]);
        }
      })
    });
  }

  protected updateWorkout(updatedWorkout: Workout) {
    this.workout = updatedWorkout;
  }
}
