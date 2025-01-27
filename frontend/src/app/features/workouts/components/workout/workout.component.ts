import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Workout} from "../../models/workout.model";
import {WorkoutService} from "../../services/workout.service";
import {SetService} from "../../services/set.service";
import {Set} from "../../models/set.model";

@Component({
  selector: 'app-workout',
  templateUrl: './workout.component.html',
  styleUrl: './workout.component.scss'
})
export class WorkoutComponent implements OnInit {
  workout!: Workout;

  constructor(
    private workoutService: WorkoutService,
    private setService: SetService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const workoutUrl = params["workout-name"];
      this.workoutService.getWorkoutByUrl(workoutUrl).subscribe(result => {
        if (result) {
          this.workout = result
        } else {
          this.router.navigate(["/not-found"]);
        }
      })
    });
  }

  markSetAsCompleted(exerciseIndex: number, setIndex: number): void {
    const set = this.workout.exercises[exerciseIndex].sets[setIndex];
    this.setService.markAsCompleted(set.id).subscribe(result => set.completed = result);
  }

  markSetAsUncompleted(exerciseIndex: number, setIndex: number): void {
    const set = this.workout.exercises[exerciseIndex].sets[setIndex];
    this.setService.markAsUncompleted(set.id).subscribe(result => set.completed = !result);
  }

  addSet(exerciseId: string, reps: number, weight: number) {
    const tempId = "temp" + Date.now();
    const newSet: Set = {
      id: tempId,
      reps: reps,
      weight: weight,
      completed: false
    };

    const exercise = this.workout.exercises.find(e => e.id === exerciseId);
    if (!exercise) {
      console.error(`Exercise with ID ${exerciseId} not found.`);
      return;
    }

    exercise.sets.push(newSet);

    this.setService.add(exerciseId, reps, weight).subscribe(response => {
      const set = exercise.sets.find(s => s.id === tempId);
      if (set) {
        set.id = response.id;
      } else {
        console.error(`Temporary set with ID ${tempId} not found.`);
      }
    });
  }

  isWorkoutCompleted(): boolean {
    return this.workout.exercises.every(exercise => exercise.sets.every(set => set.completed));
  }
}
