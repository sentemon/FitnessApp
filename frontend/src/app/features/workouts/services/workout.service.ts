import {Injectable} from '@angular/core';
import {Workout} from "../models/workout.model";
import {BehaviorSubject, map, Observable, of, tap} from "rxjs";
import {WorkoutHistory} from "../models/workout-history.model";
import {Apollo, ApolloBase, MutationResult} from "apollo-angular";
import {CREATE_WORKOUT} from "../graphql/mutations.graphql";
import {MutationResponse} from "../graphql/mutation.response";
import {QueryResponse} from "../graphql/query.response";
import {GET_ALL_WORKOUTS, GET_WORKOUT_BY_URL} from "../graphql/queries.graphql";
import {CreateWorkout} from "../models/create-workout.model";

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private workoutsSubject = new BehaviorSubject<Workout[]>([]);
  private workouts$ = this.workoutsSubject.asObservable();

  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts")

    this.workoutClient.query<QueryResponse>({
      query: GET_ALL_WORKOUTS
    }).pipe(
      tap(response => {
        let workouts = response.data.allWorkouts;

        workouts.forEach(post => this.addWorkout(post));
      })
    ).subscribe();
  }

  public getAll(): Observable<Workout[]> {
    return this.workouts$;
  }

  public getWorkoutByUrl(url: string): Observable<Workout> {
    return this.workoutClient.query<QueryResponse>({
      query: GET_WORKOUT_BY_URL,
      variables: { url }
    }).pipe(
      map(response => response.data.workoutByUrl)
    );
  }

  public create(createWorkout: CreateWorkout): Observable<Workout | MutationResult<MutationResponse>> {
    return this.workoutClient.mutate<MutationResponse>({
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
      map(response => {
        const workout = response.data?.createWorkout;

        if (workout) {
          return workout;
        }

        return response;
      })
    );
  }

  public addWorkout(workout: Workout): void {
    this.workoutsSubject.next([workout, ...this.workoutsSubject.value]);
  }

  public getWorkoutsHistory(): Observable<WorkoutHistory[]> {
    return of();
  }
}
