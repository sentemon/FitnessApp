import { Component } from '@angular/core';

@Component({
  selector: 'app-layout',
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
  `
})
export class LayoutComponent {

}
