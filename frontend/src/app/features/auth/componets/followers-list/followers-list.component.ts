import {Component, EventEmitter, Input, Output} from '@angular/core';
import {User} from "../../models/user.model";

@Component({
  selector: 'app-followers-list',
  templateUrl: './followers-list.component.html',
  styleUrl: './followers-list.component.scss'
})
export class FollowersListComponent {
  @Output() close = new EventEmitter<void>();
  @Input() followers!: User[];
}
