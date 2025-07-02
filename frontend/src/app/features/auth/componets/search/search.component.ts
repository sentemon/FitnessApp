import { Component } from '@angular/core';
import {FormControl} from "@angular/forms";
import {UserService} from "../../services/user.service";
import {debounceTime, distinctUntilChanged, switchMap} from "rxjs";
import {User} from "../../models/user.model";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent {
  searchControl = new FormControl('');
  users: User[] = [];

  constructor(private userService: UserService) {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => this.userService.searchUsers(query || ''))
    ).subscribe(result => this.users = result.response!);
  }
}
