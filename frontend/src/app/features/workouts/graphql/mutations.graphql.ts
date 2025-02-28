import gql from "graphql-tag";

export const SET_UP_PROFILE = gql`
  mutation SetUpProfile(
    $weight: Float!,
    $height: Float!,
    $goal: Goal!,
    $activityLevel: ActivityLevel!,
    $dateOfBirth: DateTime,
    $favoriteWorkoutTypes: [WorkoutType!]!
  ) {
    setUpProfile(
      input: {
        weight: $weight
        height: $height
        goal: $goal
        activityLevel: $activityLevel
        dateOfBirth: $dateOfBirth
        favoriteWorkoutTypes: $favoriteWorkoutTypes
      }
    )
  }
`;
export const CREATE_WORKOUT = gql`
  mutation CreateWorkout(
    $title: String!,
    $description: String!,
    $durationInMinutes: Int!,
    $level: DifficultyLevel!,
    $imageUrl: Upload,
    $exercises: [CreateExerciseDtoInput!]!
  ) {
    createWorkout(
      input: {
        title: $title
        description: $description
        durationInMinutes: $durationInMinutes
        level: $level
        imageUrl: $imageUrl
        exercises: $exercises
      }
    ) {
      id
      title
      description
      durationInMinutes
      level
      imageUrl
    }
  }
`;

export const ADD_WORKOUT_HISTORY = gql`
  mutation AddWorkoutHistory($workoutId: String!) {
    addWorkoutHistory(workoutId: $workoutId) {
      id
      workoutId
      userId
    }
  }
`;
