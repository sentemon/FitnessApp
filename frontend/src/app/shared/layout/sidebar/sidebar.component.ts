import { Component } from '@angular/core';
import {CookieService} from "../../../core/services/cookie.service";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SideBarComponent {
  username: string = "";

  constructor(cookieService: CookieService) {
    let result = cookieService.get("username");
    if (result.isSuccess) {
      this.username = result.response;
    }
  }
}
