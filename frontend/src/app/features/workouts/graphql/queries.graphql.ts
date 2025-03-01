import gql from "graphql-tag";

export const PROFILE_SET_UP = gql`
  query ProfileSetUp {
    profileSetUp
  }
`;

export const GET_ALL_WORKOUTS = gql`
  query AllWorkouts {
    allWorkouts {
      id
      title
      description
      durationInMinutes
      level
      isCustom
      imageUrl
      url
      exercises {
        id
        name
        level
        sets {
          id
          reps
          weight
        }
      }
    }
  }
`;

export const GET_WORKOUT_BY_URL = gql`
  query WorkoutByUrl($url: String!) {
    workoutByUrl(url: $url) {
      id
      title
      description
      durationInMinutes
      level
      isCustom
      url
      imageUrl
      exercises {
        id
        name
        level
        sets {
          id
          reps
          weight
        }
      }
    }
  }
`;

// export const MARK_SET_HISTORY_AS_COMPLETED = gql`
//
// `;
//
//
// export const MARK_SET_HISTORY_AS_UNCOMPLETED = gql`
//
// `;
