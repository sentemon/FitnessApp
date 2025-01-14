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
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    PostsModule,
    SharedModule,
    HttpClientModule,
    ApolloModule
  ],
  providers: [
    {
      provide: APOLLO_NAMED_OPTIONS,
      deps: [HttpLink],
      useFactory: graphql
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

