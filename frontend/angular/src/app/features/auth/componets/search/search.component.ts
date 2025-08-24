import { Component } from '@angular/core';
import {FormControl} from "@angular/forms";
import {UserService} from "../../services/user.service";
import {debounceTime, distinctUntilChanged, switchMap} from "rxjs";
import {UserDto} from "../../models/user-dto.model";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss'
})
export class SearchComponent {
  searchControl = new FormControl('');
  users: UserDto[] = [];

  constructor(private userService: UserService) {
    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => this.userService.searchUsers(query || ''))
    ).subscribe(result => this.users = result?.response ?? []);
  }
}
