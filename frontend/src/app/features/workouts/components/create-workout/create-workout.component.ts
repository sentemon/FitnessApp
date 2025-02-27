import {Component} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Level} from "../../models/level.model";
import {WorkoutService} from "../../services/workout.service";
import {CreateWorkout} from "../../models/create-workout.model";

@Component({
  selector: 'app-create-workout',
  templateUrl: './create-workout.component.html',
  styleUrls: ['./create-workout.component.scss']
})
export class CreateWorkoutComponent {
  workoutForm!: FormGroup;

  constructor(private fb: FormBuilder, private workoutService: WorkoutService) {
    this.workoutForm = this.fb.group({
      workoutTitle: ['test', Validators.required],
      workoutDescription: ['test', Validators.required],
      workoutImage: [null],
      workoutDurationInMinutes: [5, Validators.required],
      workoutLevel: [Level.Beginner],
      exercises: this.fb.array([this.addExercise()])
    });
  }

  getExercises(): FormArray {
    return this.workoutForm.get('exercises') as FormArray;
  }

  getSets(exerciseIndex: number): FormArray {
    return this.getExercises().at(exerciseIndex).get('sets') as FormArray;
  }

  addExercise() {
    return this.fb.group({
      name: ['test', Validators.required],
      level: [Level.Beginner],
      sets: this.fb.array([this.createSet()])
    });
  }

  createSet() {
    return this.fb.group({
      reps: [1, [Validators.required, Validators.min(1)]],
      weight: [0, [Validators.required, Validators.min(0)]]
    });
  }

  addSet(exerciseIndex: number) {
    const sets = this.getSets(exerciseIndex);
    sets.push(this.createSet());
  }

  addNewExercise() {
    this.getExercises().push(this.addExercise());
  }

  submitWorkout() {
    if (!this.workoutForm.valid) {
      console.log('Form is invalid');
      return
    }

    const workoutData: CreateWorkout = {
      title: this.workoutForm.get("workoutTitle")?.value,
      description: this.workoutForm.get("workoutDescription")?.value,
      durationInMinutes: this.workoutForm.get("workoutDurationInMinutes")?.value,
      level: this.workoutForm.get("workoutLevel")?.value,
      exercises: this.workoutForm.get("exercises")?.value
    };

    this.workoutService.create(workoutData).subscribe(response => {
      console.log('Workout created successfully', response);
    });
  }

  protected readonly Level = Level;
}
