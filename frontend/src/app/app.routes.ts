import { Routes } from '@angular/router';
import {AuthGuard} from "./core/services/auth-guard.service";
import {AppComponent} from "./app.component";

export const routes: Routes = [
  // { path: '', component: AppComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
