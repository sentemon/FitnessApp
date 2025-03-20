import { NgModule } from '@angular/core';
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
import { ChatSidebarComponent } from './features/chats/components/chat-sidebar/chat-sidebar.component';
import { ChatAreaComponent } from './features/chats/components/chat-area/chat-area.component';

@NgModule({
  declarations: [
    AppComponent,
    ChatSidebarComponent,
    ChatAreaComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    PostsModule,
    WorkoutsModule,
    SharedModule,
    HttpClientModule,
    ApolloModule
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
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

