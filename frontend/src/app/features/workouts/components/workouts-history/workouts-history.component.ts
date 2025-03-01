import {Component, OnInit} from '@angular/core';
import {WorkoutService} from "../../services/workout.service";
import {WorkoutHistory} from "../../models/workout-history.model";

@Component({
  selector: 'app-workouts-history',
  templateUrl: './workouts-history.component.html',
  styleUrl: './workouts-history.component.scss'
})
export class WorkoutsHistoryComponent implements OnInit {
  workoutHistories: WorkoutHistory[] = [];
  constructor(private workoutService: WorkoutService) { }

  ngOnInit() {
    this.workoutService.getWorkoutsHistory().subscribe(result => this.workoutHistories = result);
  }
}
