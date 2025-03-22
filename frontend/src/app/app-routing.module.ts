import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {AuthGuard} from "./core/services/auth-guard.service";
import {LoginComponent} from "./features/auth/componets/login/login.component";
import {RegisterComponent} from "./features/auth/componets/register/register.component";
import {ProfileComponent} from "./features/auth/componets/profile/profile.component";
import {CreatePostComponent} from "./features/posts/components/create-post/create-post.component";
import {PostListComponent} from "./features/posts/components/post-list/post-list.component";
import {SetUpProfileComponent} from "./features/workouts/components/set-up-profile/set-up-profile.component";
import {WorkoutsListComponent} from "./features/workouts/components/workouts-list/workouts-list.component";
import {SetUpGuard} from "./features/workouts/services/set-up.guard";
import {WorkoutComponent} from "./features/workouts/components/workout/workout.component";
import {NotFoundComponent} from "./shared/not-found/not-found.component";
import {NewWorkoutComponent} from "./features/workouts/components/new-workout/new-workout.component";
import {CreateWorkoutComponent} from "./features/workouts/components/create-workout/create-workout.component";
import {WorkoutsHistoryComponent} from "./features/workouts/components/workouts-history/workouts-history.component";
import {WorkoutHistoryComponent} from "./features/workouts/components/workout-history/workout-history.component";
import {ChatComponent} from "./features/chats/components/chat/chat.component";

const routes: Routes = [
  {path: '', component: PostListComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'sentemon', component: ProfileComponent },
  { path: 'create-post', component: CreatePostComponent, canActivate: [AuthGuard] },
  { path: 'workouts', component: WorkoutsListComponent, canActivate: [AuthGuard, SetUpGuard] },
  { path: 'workouts/new', component: NewWorkoutComponent, canActivate: [AuthGuard] },
  { path: 'workouts/create', component: CreateWorkoutComponent, canActivate: [AuthGuard] },
  { path: 'workouts/history', component: WorkoutsHistoryComponent, canActivate: [AuthGuard] },
  { path: 'workouts/:workout-name', component: WorkoutComponent, canActivate: [AuthGuard] },
  { path: 'workouts/:workout-name/:id', component: WorkoutHistoryComponent, canActivate: [AuthGuard] },
  { path: 'setup-profile', component: SetUpProfileComponent, canActivate: [AuthGuard] },
  { path: 'chats', component: ChatComponent, canActivate: [AuthGuard] },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
