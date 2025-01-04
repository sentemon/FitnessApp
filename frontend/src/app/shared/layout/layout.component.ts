import { Component } from '@angular/core';
import {SideBarComponent} from "./sidebar/sidebar.component";
import {MainComponent} from "./main/main.component";

@Component({
  selector: 'app-layout',
  standalone: true,
  template: `
    <div class="layout">
      <app-sidebar></app-sidebar>
      <app-main></app-main>
    </div>
  `,
  styles: `
    .layout {
      display: flex;
      overflow: hidden;
    }
  `,
  imports: [
    SideBarComponent,
    MainComponent
  ]
})
export class LayoutComponent {

}
