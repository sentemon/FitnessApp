import { Component } from '@angular/core';

@Component({
  selector: 'app-main',
  template: `
    <div class="main">
      <router-outlet></router-outlet>
    </div>
  `,
  styleUrl: './main.component.scss'
})
export class MainComponent {

}
