import { Component } from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {Level} from "../../models/level.model";

@Component({
  selector: 'app-create-workout',
  templateUrl: './create-workout.component.html',
  styleUrl: './create-workout.component.scss'
})
export class CreateWorkoutComponent {
  workoutForm!: FormGroup;

  constructor(fb: FormBuilder) {
    this.workoutForm = fb.group({
      workoutTitle: "",
      workoutDescription: "",
      workoutImage: null,
      workoutDurationInMinutes: "",
      workoutLevel: Level.Beginner,
      exerciseName: "",
      reps: 0,
      weight: 0
    });
  }

  submitWorkout() {

  }

  protected readonly Level = Level;
}
