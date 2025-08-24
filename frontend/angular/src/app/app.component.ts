import { Component } from '@angular/core';
import {AuthService} from "./features/auth/services/auth.service";
import {environment} from "../environments/environment";
import {ActivityStatusService} from "./features/auth/services/activity-status.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor(
    protected authService: AuthService,
    private activityStatusService: ActivityStatusService
  ) {
    console.log("Production", environment.production);
  }

  title = 'Fitness App';
}
