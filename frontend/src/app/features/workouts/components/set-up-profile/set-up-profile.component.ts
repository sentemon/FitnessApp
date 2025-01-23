import {Component, OnInit} from '@angular/core';
import {MenuItem} from "primeng/api";

@Component({
  selector: 'app-set-up-profile',
  templateUrl: './set-up-profile.component.html',
  styleUrl: './set-up-profile.component.scss'
})
export class SetUpProfileComponent implements OnInit{
  items: MenuItem[] = [];
  activeIndex: number = 0;

  constructor() {}

  ngOnInit() {
    this.items = [
      {
        label: "Welcome",
        command: () => this.activeIndex = 0
      },
      {
        label: "Personal",
        command: () => this.activeIndex = 1
      },
      {
        label: "Goal",
        command: () => this.activeIndex = 2
      },
      {
        label: "Activity",
        command: () => this.activeIndex = 3
      },
      {
        label: "Type",
        command: () => this.activeIndex = 4
      },
      {
        label: "Review",
        command: () => this.activeIndex = 5,
      }
    ];
  }

  next(event: Event): void {
    event.preventDefault();

    if (this.activeIndex < this.items.length - 1) {
      this.activeIndex++;
    }
  }

  back(event: Event): void {
    event.preventDefault();

    if (this.activeIndex > 0) {
      this.activeIndex--;
    }
  }

  protected readonly Event = Event;
}
