import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {WorkoutHistory} from "../../models/workout-history.model";
import {WorkoutHistoryService} from "../../services/workout-history.service";

@Component({
  selector: 'app-workout-history',
  templateUrl: './workout-history.component.html',
  styleUrl: './workout-history.component.scss'
})
export class WorkoutHistoryComponent implements OnInit {
  // ToDo: fix
  workoutHistory!: WorkoutHistory

  constructor(
    private workoutHistoryService: WorkoutHistoryService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const workoutHistoryId = params["id"];
      this.workoutHistoryService.get(workoutHistoryId).subscribe(result => {
        if (result) {
          this.workoutHistory = {
            id: result.id,
            workout: result.workout,
            durationInMinutes: result.durationInMinutes,
            exerciseHistories: result.exerciseHistories,
            performedAt: result.performedAt,
            userId: result.userId,

            // id: result.id,
            // title: result.title,
            // description: result.description,
            // durationInMinutes: result.durationInMinutes,
            // level: result.level,
            // url: result.url,
            // exercises: result.exercises,
            // imageUrl: result.imageUrl,
            // isCustom: result.isCustom,
            // userId: result.userId
          }

          console.log(result);
        } else {
          this.router.navigate(["/not-found"]);
        }
      })
    });
  }

  isWorkoutCompleted(): boolean {
    return this.workoutHistory.exerciseHistories.length > 0 &&
      this.workoutHistory.exerciseHistories.every(exerciseHistory =>
        exerciseHistory.setHistories.length > 0 &&
        exerciseHistory.setHistories.every(setHistory => setHistory.completed)
      );
  }

  protected updateWorkoutHistory(updatedWorkoutHistory: WorkoutHistory) {
    this.workoutHistory = updatedWorkoutHistory;
  }
}
