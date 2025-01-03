import { Routes } from '@angular/router';
import {AppComponent} from "./app.component";
import {LoginComponent} from "./features/auth/componets/login/login.component";
import {RegisterComponent} from "./features/auth/componets/register/register.component";

export const routes: Routes = [
  // { path: '', component: AppComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
