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
