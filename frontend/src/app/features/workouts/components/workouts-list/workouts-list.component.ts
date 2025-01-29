import {Component, OnInit} from '@angular/core';
import {WorkoutService} from "../../services/workout.service";
import {Workout} from "../../models/workout.model";

@Component({
  selector: 'app-workouts-list',
  templateUrl: './workouts-list.component.html',
  styleUrl: './workouts-list.component.scss'
})
export class WorkoutsListComponent implements OnInit {
  workouts: Workout[] = [];
  user = {
    firstName: "Ivan"
  };

  constructor(private workoutService: WorkoutService) { }

  ngOnInit() {
    this.workoutService.getAllWorkouts().subscribe(workouts => this.workouts = workouts);
  }
}
