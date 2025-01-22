import { NgModule } from '@angular/core';
import { SetUpProfileComponent } from './components/set-up-profile/set-up-profile.component';
import { StepperModule } from 'primeng/stepper';
import {NgClass} from "@angular/common";
import {Button} from "primeng/button";
import {FormsModule} from "@angular/forms";
import {Password} from "primeng/password";

@NgModule({
  declarations: [
    SetUpProfileComponent
  ],
  imports: [
    StepperModule,
    NgClass,
    Button,
    FormsModule,
    Password
  ]
})
export class WorkoutsModule { }
