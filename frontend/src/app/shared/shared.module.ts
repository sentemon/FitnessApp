import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {MainComponent} from "./layout/main/main.component";
import {SideBarComponent} from "./layout/sidebar/sidebar.component";
import {LayoutComponent} from "./layout/layout.component";
import {RouterOutlet} from "@angular/router";

@NgModule({
  declarations: [
    LayoutComponent,
    MainComponent,
    SideBarComponent
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
