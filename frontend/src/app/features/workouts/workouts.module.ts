import { NgModule } from '@angular/core';
import { SetUpProfileComponent } from './components/set-up-profile/set-up-profile.component';
import { StepperModule } from 'primeng/stepper';
import {AsyncPipe, NgForOf, NgIf, NgStyle} from "@angular/common";
import {Steps} from "primeng/steps";
import { WorkoutsListComponent } from './components/workouts-list/workouts-list.component';
import { WorkoutComponent } from './components/workout/workout.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { NewWorkoutComponent } from './components/new-workout/new-workout.component';
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../shared/shared.module";
import { SetComponent } from './components/set/set.component';
import { ExerciseComponent } from './components/exercise/exercise.component';
import { CreateWorkoutComponent } from './components/create-workout/create-workout.component';
import { WorkoutsHistoryComponent } from './components/workouts-history/workouts-history.component';
import { WorkoutHistoryComponent } from './components/workout-history/workout-history.component';
import { ExerciseHistoryComponent } from './components/exercise-history/exercise-history.component';
import { SetHistoryComponent } from './components/set-history/set-history.component';

@NgModule({
  declarations: [
    SetUpProfileComponent,
    WorkoutsListComponent,
    WorkoutComponent,
    NewWorkoutComponent,
    SetComponent,
    ExerciseComponent,
    CreateWorkoutComponent,
    WorkoutsHistoryComponent,
    WorkoutHistoryComponent,
    ExerciseHistoryComponent,
    SetHistoryComponent
  ],
  imports: [
    StepperModule,
    Steps,
    NgIf,
    NgStyle,
    NgForOf,
    FormsModule,
    RouterLink,
    SharedModule,
    ReactiveFormsModule,
  ]
})
export class WorkoutsModule { }
