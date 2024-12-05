import { Routes } from '@angular/router';
import {PostListComponent} from "./features/posts/components/post-list/post-list.component";
import {AuthGuard} from "./core/services/auth-guard.service";

export const routes: Routes = [
  { path: '', component: PostListComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];
