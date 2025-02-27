import {Injectable} from '@angular/core';
import {Workout} from "../models/workout.model";
import {Level} from "../models/level.model";
import {map, Observable, of} from "rxjs";
import {Activity} from "../models/activity.model";
import {Apollo, ApolloBase, MutationResult} from "apollo-angular";
import {CREATE_WORKOUT} from "../graphql/mutations.graphql";
import {MutationResponse} from "../graphql/mutation.response";
import {Exercise} from "../models/exercise.model";
import {QueryResponse} from "../graphql/query.response";
import {GET_ALL_WORKOUTS, GET_WORKOUT_BY_URL} from "../graphql/queries.graphql";
import {CreateWorkout} from "../models/create-workout.model";

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  // private workouts: Workout[] = [
  //   {
  //     id: '1',
  //     title: 'Boxing',
  //     description: 'This session will get your heart rate up and strengthen your core and upper body.',
  //     time: 35,
  //     level: Level.Advanced,
  //     url: 'boxing',
  //     isCustom: false,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Jab-Cross Combo',
  //         sets: [{
  //           reps: 10, weight: 0,
  //           completed: false,
  //           id: '1',
  //           exerciseId: '1'
  //         }]
  //       },
  //       {
  //         id: '2',
  //         name: 'Speed Bag',
  //         sets: [{
  //           reps: 3, weight: 0,
  //           completed: false,
  //           id: '2',
  //           exerciseId: '2'
  //         }]
  //       }
  //     ],
  //     imageUrl: ''
  //   },
  //   {
  //     id: '2',
  //     title: 'Core Pilates',
  //     description: 'This session will focus on your side body and full body integration to leave you feeling strong!',
  //     time: 30,
  //     level: Level.Beginner,
  //     url: 'core-pilates',
  //     isCustom: false,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Jab-Cross Combo',
  //         sets: [{
  //           reps: 10, weight: 0,
  //           completed: false,
  //           id: '3',
  //           exerciseId: '1'
  //         }]
  //       },
  //       {
  //         id: '2',
  //         name: 'Speed Bag',
  //         sets: [{
  //           reps: 3, weight: 0,
  //           completed: false,
  //           id: '4',
  //           exerciseId: '2'
  //         }]
  //       }
  //     ],
  //     imageUrl: ''
  //   },
  //   {
  //     id: '3',
  //     title: 'Dumbbells HIIT',
  //     description: "In this workouts-list, you'll challenge your body and target every major muscle group one by one leaving your feeling fully satisfied.",
  //     time: 35,
  //     level: Level.AllLevels,
  //     url: 'dumbblles-hiit',
  //     isCustom: false,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Jab-Cross Combo',
  //         sets: [{
  //           reps: 10, weight: 0,
  //           completed: false,
  //           id: '5',
  //           exerciseId: '1'
  //         }]
  //       },
  //       {
  //         id: '2',
  //         name: 'Speed Bag',
  //         sets: [{
  //           reps: 3, weight: 0,
  //           completed: false,
  //           id: '6',
  //           exerciseId: '2'
  //         }]
  //       }
  //     ],
  //     imageUrl: ''
  //   },
  //   {
  //     id: '4',
  //     title: 'Gratitude Meditation',
  //     description: 'A guided gratitude meditation and focus on manifesting more optimism and positivity in your life.',
  //     time: 20,
  //     level: Level.AllLevels,
  //     url: 'gratitude-meditation',
  //     isCustom: false,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Jab-Cross Combo',
  //         sets: [{
  //           reps: 10, weight: 0,
  //           completed: false,
  //           id: '7',
  //           exerciseId: '1'
  //         }]
  //       },
  //       {
  //         id: '2',
  //         name: 'Speed Bag',
  //         sets: [{
  //           reps: 3, weight: 0,
  //           completed: false,
  //           id: '8',
  //           exerciseId: '2'
  //         }]
  //       }
  //     ],
  //     imageUrl: ''
  //   },
  //   {
  //     id: '5',
  //     title: 'Vinyasa Yoga',
  //     description: 'This session focuses on alignment and balance.',
  //     time: 35,
  //     level: Level.Beginner,
  //     url: 'vinyasa-yoga',
  //     isCustom: false,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Jab-Cross Combo',
  //         sets: [{
  //           reps: 10, weight: 0,
  //           completed: false,
  //           id: '34',
  //           exerciseId: '1'
  //         }]
  //       },
  //       {
  //         id: '2',
  //         name: 'Speed Bag',
  //         sets: [{
  //           reps: 3, weight: 0,
  //           completed: false,
  //           id: '9',
  //           exerciseId: '2'
  //         }]
  //       }
  //     ],
  //     imageUrl: ''
  //   },
  //   {
  //     id: '6',
  //     title: 'Full-body HIIT',
  //     description: 'Grab your sneakers and get ready for a cardio infused full-body workouts-list.',
  //     time: 35,
  //     level: Level.Intermediate,
  //     url: 'full-body-hiit',
  //     isCustom: false,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Jab-Cross Combo',
  //         sets: [{
  //           reps: 10, weight: 0,
  //           completed: false,
  //           id: '10',
  //           exerciseId: '1'
  //         }]
  //       },
  //       {
  //         id: '2',
  //         name: 'Speed Bag',
  //         sets: [{
  //           reps: 3, weight: 0,
  //           completed: false,
  //           id: '11',
  //           exerciseId: '2'
  //         }]
  //       }
  //     ],
  //     imageUrl: ''
  //   },
  //   {
  //     id: '7',
  //     title: 'Full Body Workout',
  //     description: 'A complete full-body workout for strength and endurance.',
  //     time: 0,
  //     level: Level.Intermediate,
  //     url: 'full-body-workout',
  //     imageUrl: '',
  //     isCustom: true,
  //     exercises: [
  //       {
  //         id: '1',
  //         name: 'Push-Ups',
  //         sets: [
  //           {
  //             reps: 15, weight: 0, completed: false,
  //             id: '12',
  //             exerciseId: '1'
  //           },
  //           {
  //             reps: 15, weight: 0, completed: false,
  //             id: '13',
  //             exerciseId: '1'
  //           }
  //         ]
  //       },
  //       {
  //         id: '2',
  //         name: 'Squats',
  //         sets: [
  //           {
  //             reps: 20, weight: 0, completed: false,
  //             id: '14',
  //             exerciseId: '2'
  //           },
  //           {
  //             reps: 20, weight: 0, completed: false,
  //             id: '15',
  //             exerciseId: '2'
  //           }
  //         ]
  //       },
  //       {
  //         id: '3',
  //         name: 'Plank',
  //         sets: [
  //           {
  //             reps: 1, weight: 0, completed: false,
  //             id: '16',
  //             exerciseId: '3'
  //           }
  //         ]
  //       }
  //     ],
  //   }
  // ];

  private workoutClient: ApolloBase;

  constructor(apollo: Apollo) {
    this.workoutClient = apollo.use("workouts")
  }

  public getAll(): Observable<Workout[]> {
    return this.workoutClient.query<QueryResponse>({
      query: GET_ALL_WORKOUTS
    }).pipe(
      map(response => response.data.allWorkouts)
    );
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
    const title = createWorkout.title;
    const description = createWorkout.description;
    const durationInMinutes = createWorkout.durationInMinutes;
    const level = createWorkout.level;

    return this.workoutClient.mutate<MutationResponse>({
      mutation: CREATE_WORKOUT,
      variables: { title, description, durationInMinutes, level }
    }).pipe(
      map(response => {
        const workout = response.data?.createWorkout;

        if (workout) {
          return workout;
        }

        return response;
      })
    )
  }

  public getWorkoutsHistory(): Observable<Activity[]> {
    return of([
      {
        id: "asd",
        sport: "asd",
        title: "asd",
        time: 40
      },
      {
        id: "asd",
        sport: "asd",
        title: "asd",
        time: 40
      },
      {
        id: "asd",
        sport: "asd",
        title: "asd",
        time: 40
      }
    ])
  }
}
