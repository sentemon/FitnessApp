import {Component } from '@angular/core';
import {UserService} from "../../services/user.service";
import {User} from "../../models/user.model";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  user?: User;

  constructor(private userService: UserService) { }

  getSentemon(): void {
    this.userService.getUserByUsername("sentemon").subscribe(result => {
      this.user = result;
    });
    console.log(this.user);
  }

}
