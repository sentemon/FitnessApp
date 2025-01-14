import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {MainComponent} from "./layout/main/main.component";
import {SideBarComponent} from "./layout/sidebar/sidebar.component";
import {PostsModule} from "../features/posts/posts.module";
import {LayoutComponent} from "./layout/layout.component";



@NgModule({
  declarations: [
    LayoutComponent,
    MainComponent,
    SideBarComponent
  ],
  exports: [
    SideBarComponent,
    MainComponent
  ],
  imports: [
    CommonModule,
    PostsModule,
    NgOptimizedImage
  ]
})
export class SharedModule { }
