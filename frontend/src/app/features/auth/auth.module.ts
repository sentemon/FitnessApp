import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LoginComponent} from "./componets/login/login.component";
import {ReactiveFormsModule} from "@angular/forms";
import {RouterLink} from "@angular/router";
import {LogoutComponent} from "./componets/logout/logout.component";
import {ProfileComponent} from "./componets/profile/profile.component";
import {RegisterComponent} from "./componets/register/register.component";
import {PostsModule} from "../posts/posts.module";
import {SearchComponent} from "./componets/search/search.component";
import { FollowersListComponent } from './componets/followers-list/followers-list.component';
import {FollowingListComponent} from "./componets/following-list/following-list.component";
import { SettingsComponent } from './componets/settings/settings.component';
import { ResetPasswordComponent } from './componets/reset-password/reset-password.component';



@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
    ProfileComponent,
    RegisterComponent,
    SearchComponent,
    FollowersListComponent,
    FollowingListComponent,
    SettingsComponent,
    ResetPasswordComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    PostsModule,
  ]
})
export class AuthModule { }
