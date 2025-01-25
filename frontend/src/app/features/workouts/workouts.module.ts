import { NgModule } from '@angular/core';
import { SetUpProfileComponent } from './components/set-up-profile/set-up-profile.component';
import { StepperModule } from 'primeng/stepper';
import {NgForOf, NgIf, NgStyle} from "@angular/common";
import {Steps} from "primeng/steps";
import { WorkoutsListComponent } from './components/workout/workouts-list.component';

@NgModule({
  declarations: [
    SetUpProfileComponent,
    WorkoutsListComponent
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
