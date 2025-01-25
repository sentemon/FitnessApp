import { NgModule } from '@angular/core';
import { SetUpProfileComponent } from './components/set-up-profile/set-up-profile.component';
import { StepperModule } from 'primeng/stepper';
import {NgClass, NgForOf, NgIf, NgStyle} from "@angular/common";
import {Button} from "primeng/button";
import {FormsModule} from "@angular/forms";
import {Password} from "primeng/password";
import {Steps} from "primeng/steps";
import { WorkoutComponent } from './components/workout/workout.component';

@NgModule({
  declarations: [
    SetUpProfileComponent,
    WorkoutComponent
  ],
  imports: [
    StepperModule,
    Steps,
    NgIf,
    NgStyle,
    NgForOf
  ]
})
export class WorkoutsModule { }
