import { Component } from '@angular/core';
import {AuthService} from "./features/auth/services/auth.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor(protected authService: AuthService) { }
  title = 'Fitness App';
}
