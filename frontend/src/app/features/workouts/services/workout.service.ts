import {Injectable} from '@angular/core';
import {Workout} from "../models/workout.model";
import {Level} from "../models/level.model";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private workouts: Workout[] = [
    {
      id: '1',
      title: 'Boxing',
      description: 'This session will get your heart rate up and strengthen your core and upper body.',
      time: 35,
      level: Level.Advanced,
      imageUrl: ''
    },
    {
      id: '2',
      title: 'Core Pilates',
      description: 'This session will focus on your side body and full body integration to leave you feeling strong!',
      time: 30,
      level: Level.Beginner,
      imageUrl: ''
    },
    {
      id: '3',
      title: 'Dumbbells HIIT',
      description: "In this workout, you'll challenge your body and target every major muscle group one by one leaving your feeling fully satisfied.",
      time: 35,
      level: Level.AllLevels,
      imageUrl: ''
    },
    {
      id: '4',
      title: 'Gratitude Meditation',
      description: 'A guided gratitude meditation and focus on manifesting more optimism and positivity in your life.',
      time: 20,
      level: Level.AllLevels,
      imageUrl: ''
    },
    {
      id: '5',
      title: 'Vinyasa Yoga',
      description: 'This session focuses on alignment and balance.',
      time: 35,
      level: Level.Beginner,
      imageUrl: ''
    },
    {
      id: '6',
      title: 'Full-body HIIT',
      description: 'Grab your sneakers and get ready for a cardio infused full-body workout.',
      time: 35,
      level: Level.Intermediate,
      imageUrl: ''
    }
  ];

  constructor() { }

  public getAllWorkouts(): Observable<Workout[]> {
    return of(this.workouts);
  }
}
