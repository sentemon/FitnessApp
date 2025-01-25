import {Component, OnInit} from '@angular/core';
import {WorkoutService} from "../../services/workout.service";
import {Workout} from "../../models/workout.model";

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrl: './workout.component.scss'
})
export class WorkoutComponent implements OnInit {
  workouts: Workout[] = [];
  user = {
    firstName: "Ivan"
  };

  constructor(private workoutService: WorkoutService) { }

  ngOnInit() {
    this.workoutService.getAllWorkouts().subscribe(workouts => this.workouts = workouts);
  }
}
