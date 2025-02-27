import {Component} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Level} from "../../models/level.model";
import {WorkoutService} from "../../services/workout.service";

@Component({
  selector: 'app-create-workout',
  templateUrl: './create-workout.component.html',
  styleUrls: ['./create-workout.component.scss']
})
export class CreateWorkoutComponent {
  workoutForm!: FormGroup;

  constructor(private fb: FormBuilder, private workoutService: WorkoutService) {
    this.workoutForm = this.fb.group({
      workoutTitle: ['', Validators.required],
      workoutDescription: ['', Validators.required],
      workoutImage: [null],
      workoutDurationInMinutes: ['', Validators.required],
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
      name: ['', Validators.required],
      level: [Level.Beginner],
      sets: this.fb.array([this.createSet()])
    });
  }

  createSet() {
    return this.fb.group({
      reps: [0, [Validators.required, Validators.min(1)]],
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
    if (this.workoutForm.valid) {
      const workoutData = this.workoutForm.value;
      this.workoutService.create(workoutData).subscribe(response => {
        console.log('Workout created successfully', response);
      });
    } else {
      console.log('Form is invalid');
    }
  }

  protected readonly Level = Level;
}
