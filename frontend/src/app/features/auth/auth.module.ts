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



@NgModule({
  declarations: [
    LoginComponent,
    LogoutComponent,
    ProfileComponent,
    RegisterComponent,
    SearchComponent,
    FollowersListComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    PostsModule,
  ]
})
export class AuthModule { }
