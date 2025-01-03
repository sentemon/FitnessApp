import {Component, OnInit} from '@angular/core';
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  user: any;

  constructor(private userService: UserService) { }

  getSentemon(): void {
    this.userService.getUserByUsername("sentemon").subscribe(user => {
      this.user = user;
    });
    console.log(this.user);
  }

}
