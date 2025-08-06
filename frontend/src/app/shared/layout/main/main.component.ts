import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-main',
  template: `
    <div class="main" [ngStyle]="{ width: isSidebarOpen ? '80vw' : '100vw' }">
      <router-outlet></router-outlet>
    </div>
  `,
  styleUrl: './main.component.scss'
})
export class MainComponent {
  @Input() isSidebarOpen!: boolean;
}
