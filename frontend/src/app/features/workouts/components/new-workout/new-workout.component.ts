import {Component, OnInit} from '@angular/core';
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";
import {Router} from "@angular/router";
import {WorkoutHistoryService} from "../../services/workout-history.service";

@Component({
  selector: 'app-new-workout',
  templateUrl: './new-workout.component.html',
  styleUrl: './new-workout.component.scss'
})
export class NewWorkoutComponent implements OnInit {
  workouts: Workout[] = [];
  selectedWorkoutId: string | null = null;

  constructor(private workoutService: WorkoutService, private workoutHistoryService: WorkoutHistoryService, private router: Router) { }

  ngOnInit(): void {
    this.workoutService.getAll().subscribe(result => this.workouts = result);
  }

  onAddWorkout(): void {
    let selectedWorkout = this.workouts.find(w => w.id == this.selectedWorkoutId);

    this.workoutHistoryService.add(selectedWorkout!.id).subscribe();
    // todo: сделать так чтобы перекидывало на новую начатую тренировку (типо workouts/full-body/{id})
    this.router.navigate([`/workouts/${selectedWorkout?.url}`]);
  }
}
