import { Component } from '@angular/core';
import {AuthService} from "./features/auth/services/auth.service";
import {environment} from "../environments/environment";
import {PlatformService} from "./core/services/platform.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor(
    protected authService: AuthService,
    platformService: PlatformService,
  ) {
    console.log("Production", environment.production);
    console.log("Platform is web", platformService.isWeb());
  }

  title = 'Fitness App';
}
