import { Component, OnInit } from '@angular/core';
import {CookieService} from "../../../core/services/cookie.service";
import {UserService} from "../../../features/auth/services/user.service";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SideBarComponent implements OnInit {
  username: string = "";

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
        this.username = result.response.username.value;
      }
    });
  }
}
