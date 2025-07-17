import {Component, EventEmitter, Input, Output} from '@angular/core';
import {User} from "../../models/user.model";

@Component({
  selector: 'app-following-list',
  templateUrl: './following-list.component.html',
  styleUrl: './following-list.component.scss'
})
export class FollowingListComponent {
  @Output() close = new EventEmitter<void>();
  @Input() following!: User[];

  closeModal(): void {
    this.close.emit();
  }
}
