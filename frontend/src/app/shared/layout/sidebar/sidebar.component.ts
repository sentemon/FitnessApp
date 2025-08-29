import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {StorageService} from "../../../core/services/storage.service";
import {UserService} from "../../../features/auth/services/user.service";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SideBarComponent implements OnInit {
  username: string = "";

  @Output() linkClicked = new EventEmitter<void>();

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(result => {
      if (result.isSuccess) {
        this.username = result.response.username.value;
      }
    });
  }

  onLinkClick() {
    this.linkClicked.emit();
  }
}
