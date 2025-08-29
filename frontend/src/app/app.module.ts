import {APP_INITIALIZER, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpLink} from "apollo-angular/http";
import {APOLLO_NAMED_OPTIONS, ApolloModule} from "apollo-angular";

import {AuthModule} from "./features/auth/auth.module";
import {PostsModule} from "./features/posts/posts.module";
import {SharedModule} from "./shared/shared.module";
import {graphql} from "./graphql.config";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {AuthInterceptor} from "./core/services/auth.interceptor";
import {WorkoutsModule} from "./features/workouts/workouts.module";
import {FormsModule} from "@angular/forms";
import {ChatsModule} from "./features/chats/chats.module";
import {StorageService} from "./core/services/storage.service";
import {ActivityStatusService} from "./features/auth/services/activity-status.service";

export function initStorage(service: StorageService) {
  return () => service.init();
}

export function initActivity(service: ActivityStatusService) {
  return () => service.init();
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    PostsModule,
    WorkoutsModule,
    ChatsModule,
    SharedModule,
    HttpClientModule,
    ApolloModule,
    FormsModule
  ],
  providers: [
    {
      provide: APOLLO_NAMED_OPTIONS,
      deps: [HttpLink],
      useFactory: graphql
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      deps: [StorageService],
      useFactory: initStorage,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      deps: [ActivityStatusService],
      useFactory: initActivity,
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

