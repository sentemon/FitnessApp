import {Component, OnInit} from '@angular/core';
import {WorkoutService} from "../../services/workout.service";
import {Workout} from "../../models/workout.model";
import {Activity} from "../../models/workout-history.model";

@Component({
  selector: 'app-activities-history',
  templateUrl: './activities-history.component.html',
  styleUrl: './activities-history.component.scss'
})
export class ActivitiesHistoryComponent implements OnInit {
  activities: Activity[] = [];
  constructor(private workoutService: WorkoutService) { }

  ngOnInit() {
    this.workoutService.getWorkoutsHistory().subscribe(result => this.activities = result);
  }
}
