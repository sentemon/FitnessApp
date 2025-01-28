import { NgModule } from '@angular/core';
import { SetUpProfileComponent } from './components/set-up-profile/set-up-profile.component';
import { StepperModule } from 'primeng/stepper';
import {NgForOf, NgIf, NgStyle} from "@angular/common";
import {Steps} from "primeng/steps";
import { WorkoutsListComponent } from './components/workouts-list/workouts-list.component';
import { WorkoutComponent } from './components/workout/workout.component';
import {FormsModule} from "@angular/forms";
import { NewWorkoutComponent } from './components/new-workout/new-workout.component';
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../shared/shared.module";

@NgModule({
  declarations: [
    SetUpProfileComponent,
    WorkoutsListComponent,
    WorkoutComponent,
    NewWorkoutComponent
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
  ]
})
export class WorkoutsModule { }
