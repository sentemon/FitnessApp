import { Routes } from '@angular/router';
import {AppComponent} from "./app.component";

export const routes: Routes = [
  // { path: '', component: AppComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
