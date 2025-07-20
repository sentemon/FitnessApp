import {Component, OnInit} from '@angular/core';
import {WorkoutHistory} from "../../models/workout-history.model";
import {WorkoutHistoryService} from "../../services/workout-history.service";

@Component({
  selector: 'app-workouts-history',
  templateUrl: './workouts-history.component.html',
  styleUrl: './workouts-history.component.scss'
})
export class WorkoutsHistoryComponent implements OnInit {
  workoutHistories: WorkoutHistory[] = [];
  constructor(private workoutHistoryService: WorkoutHistoryService) { }

  ngOnInit() {
    this.workoutHistoryService.getAll().subscribe(result => {
      if (result.isSuccess) {
        this.workoutHistories = result.response;
      }
    });
  }
}
