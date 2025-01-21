import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LoginComponent} from "./componets/login/login.component";
import {ReactiveFormsModule} from "@angular/forms";
import {RouterLink} from "@angular/router";
import {LogoutComponent} from "./componets/logout/logout.component";
import {ProfileComponent} from "./componets/profile/profile.component";
import {RegisterComponent} from "./componets/register/register.component";



@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
    ProfileComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ]
})
export class AuthModule { }
