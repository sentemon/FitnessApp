import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {MainComponent} from "./layout/main/main.component";
import {SideBarComponent} from "./layout/sidebar/sidebar.component";
import {LayoutComponent} from "./layout/layout.component";
import {RouterOutlet} from "@angular/router";
import { NotFoundComponent } from './not-found/not-found.component';

@NgModule({
  declarations: [
    LayoutComponent,
    MainComponent,
    SideBarComponent,
    NotFoundComponent
  ],
  exports: [
    LayoutComponent
  ],
  imports: [
    CommonModule,
    NgOptimizedImage,
    RouterOutlet
  ]
})
export class SharedModule { }
