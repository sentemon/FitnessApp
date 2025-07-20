import {Injectable} from '@angular/core';
import {Workout} from "../models/workout.model";
import {BehaviorSubject, map, Observable, of, tap} from "rxjs";
import {WorkoutHistory} from "../models/workout-history.model";
import {Apollo, ApolloBase, MutationResult} from "apollo-angular";
import {CREATE_WORKOUT} from "../graphql/mutations.graphql";
import {GET_ALL_WORKOUTS, GET_WORKOUT_BY_URL} from "../graphql/queries.graphql";
import {CreateWorkout} from "../models/create-workout.model";
import {toResult} from "../../../core/extensions/graphql-result-wrapper";
import {Result} from "../../../core/types/result/result.type";

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private workoutsSubject = new BehaviorSubject<Workout[]>([]);
  private workouts$ = this.workoutsSubject.asObservable();

  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts")

    this.workoutClient.query({
      query: GET_ALL_WORKOUTS
    }).pipe(
      toResult<Workout[]>("allWorkouts"),
      tap(result => {
        let workouts = result.response!;

        workouts.forEach(post => this.addWorkout(post));
      })
    ).subscribe();
  }

  public getAll(): Observable<Workout[]> {
    return this.workouts$;
  }

  public getWorkoutByUrl(url: string): Observable<Result<Workout>> {
    return this.workoutClient.query({
      query: GET_WORKOUT_BY_URL,
      variables: { url }
    }).pipe(
      toResult<Workout>('workoutByUrl')
    );
  }

  public create(createWorkout: CreateWorkout): Observable<Result<Workout>> {
    return this.workoutClient.mutate({
      mutation: CREATE_WORKOUT,
      variables: {
        title: createWorkout.title,
        description: createWorkout.description,
        durationInMinutes: createWorkout.durationInMinutes,
        level: createWorkout.level,
        imageUrl: null,
        exercises: createWorkout.exercises
      }
    }).pipe(
      toResult<Workout>('createWorkout')
    );
  }

  public addWorkout(workout: Workout): void {
    this.workoutsSubject.next([workout, ...this.workoutsSubject.value]);
  }
}
