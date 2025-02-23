import {Component, OnInit} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-new-workout',
  templateUrl: './new-workout.component.html',
  styleUrl: './new-workout.component.scss'
})
export class NewWorkoutComponent implements OnInit {
  workouts: Workout[] = [];
  selectedWorkoutId: string | null = null;

  constructor(private workoutService: WorkoutService, private router: Router) { }

  ngOnInit(): void {
    this.workoutService.getAll().subscribe(result => this.workouts = result);
  }

  onAddWorkout(): void {
    let selectedWorkoutUrl = this.workouts.find(w => w.id == this.selectedWorkoutId)?.url;

    this.router.navigate([`/workouts/${selectedWorkoutUrl}`]);
  }
}
