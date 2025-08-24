import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import {UserService} from "../../services/user.service";
import { Goal } from '../../models/goal.model';
import {ActivityLevel} from "../../models/activity-level.model";
import {WorkoutType} from "../../models/workout-type.model";
import {Router} from "@angular/router";

@Component({
  selector: 'app-set-up-profile',
  templateUrl: './set-up-profile.component.html',
  styleUrls: ['./set-up-profile.component.scss'],
})
export class SetUpProfileComponent implements OnInit {
  items: MenuItem[] = [];
  activeIndex: number = 0;

  weight!: number;
  height!: number;
  goal!: Goal;
  activityLevel!: ActivityLevel;
  dateOfBirth: Date | null = null;
  favoriteWorkoutTypes: WorkoutType[] = [];

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() {
    this.items = [
      { label: 'Welcome', command: () => (this.activeIndex = 0) },
      { label: 'Personal', command: () => (this.activeIndex = 1) },
      { label: 'Goal', command: () => (this.activeIndex = 2) },
      { label: 'Activity', command: () => (this.activeIndex = 3) },
      { label: 'Type', command: () => (this.activeIndex = 4) },
      { label: 'Review', command: () => (this.activeIndex = 5) },
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

  confirm(): void {
    this.userService.setUpProfile(
      this.weight,
      this.height,
      this.goal,
      this.activityLevel,
      this.dateOfBirth ? new Date(this.dateOfBirth).toISOString() : null,
      // ToDo: Fix
      []
      // this.favoriteWorkoutTypes
    ).subscribe(result => {
      console.log(result);
    });

    this.router.navigate(["/workouts"]);
  }

  protected readonly Goal = Goal;
  protected readonly ActivityLevel = ActivityLevel;
  protected readonly WorkoutType = WorkoutType;
}
