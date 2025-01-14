import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LayoutComponent} from "./shared/layout/layout.component";
import {AuthGuard} from "./core/services/auth-guard.service";
import {LoginComponent} from "./features/auth/componets/login/login.component";
import {RegisterComponent} from "./features/auth/componets/register/register.component";
import {ProfileComponent} from "./features/auth/componets/profile/profile.component";

const routes: Routes = [
  {path: '', component: LayoutComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'sentemon', component: ProfileComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
