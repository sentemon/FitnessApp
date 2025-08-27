import { NgModule } from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {MainComponent} from "./layout/main/main.component";
import {SideBarComponent} from "./layout/sidebar/sidebar.component";
import {LayoutComponent} from "./layout/layout.component";
import {RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import { NotFoundComponent } from './not-found/not-found.component';
import { BackComponent } from './back/back.component';

@NgModule({
  declarations: [
    LayoutComponent,
    MainComponent,
    SideBarComponent,
    NotFoundComponent,
    BackComponent
  ],
  exports: [
    LayoutComponent,
    BackComponent
  ],
  imports: [
    CommonModule,
    NgOptimizedImage,
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ]
})
export class SharedModule { }
